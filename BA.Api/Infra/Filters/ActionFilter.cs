using BA.Api.Infra.Model;
using BA.Utility.Constant;
using BA.Utility.Content;
using BA.Utility.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

namespace BA.Api.Infra.Filters
{
    public class ActionFilter : IActionFilter, IExceptionFilter
    {
        private readonly ILogger<ActionFilter> _logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(kvp => new Errors
                    {
                        PropertyName = kvp.Key,
                        ErrorMessages = kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToList();

                context.Result = new JsonResult(new ResponseModel
                {
                    StatusCode = (int)ApiStatusCodeEnum.PreconditionFailed,
                    Message = ContentLoader.ReturnLanguageData("BA510", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                    Data = null,
                    Errors = errors
                });

                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;

            if (context.Result is FileContentResult)
                return;

            if (context.Result is JsonResult jsonResult && jsonResult.Value is ResponseModel)
                return;

            ResponseModel response;

            switch (context.Result)
            {
                case ObjectResult objectResult when objectResult.Value is ResponseModel responseModel:
                    response = responseModel;
                    break;

                case OkObjectResult okResult:
                    response = new ResponseModel
                    {
                        StatusCode = (int)ApiStatusCodeEnum.Success,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = okResult.Value,
                        Errors = new List<Errors>()
                    };
                    break;

                case ObjectResult objectResult:
                    response = new ResponseModel
                    {
                        StatusCode = (int)ApiStatusCodeEnum.Success,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = objectResult.Value,
                        Errors = new List<Errors>()
                    };
                    break;

                case OkResult:
                case EmptyResult:
                    response = new ResponseModel
                    {
                        StatusCode = (int)ApiStatusCodeEnum.Success,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = null,
                        Errors = new List<Errors>()
                    };
                    break;

                case UnauthorizedResult:
                    response = new ResponseModel
                    {
                        StatusCode = (int)ApiStatusCodeEnum.Unauthorized,
                        Message = ContentLoader.ReturnLanguageData("BA511", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = null,
                        Errors = new List<Errors>()
                    };
                    break;

                default:
                    response = new ResponseModel
                    {
                        StatusCode = (int)ApiStatusCodeEnum.Success,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = context.Result,
                        Errors = new List<Errors>()
                    };
                    break;
            }

            context.Result = new JsonResult(response);
        }

        public void OnException(ExceptionContext context)
        {
            var lang = Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD]);

            if (context.Exception is SqlException sqlEx)
            {
                var errorKey = sqlEx.Number switch
                {
                    17 => "BA551",
                    4060 => "BA552",
                    18456 => "BA553",
                    53 => "BA554",
                    -2 => "BA555",
                    1205 => "BA556",
                    2601 or 2627 => "BA557",
                    _ => "BA516"
                };

                var response = new ResponseModel
                {
                    StatusCode = (int)ApiStatusCodeEnum.InternalServerError,
                    Message = ContentLoader.ReturnLanguageData(errorKey, lang),
                    Data = null,
                    Errors = new List<Errors>()
                };

                context.Result = new JsonResult(response);
                context.ExceptionHandled = true;

                _logger.LogError(sqlEx,
                    "SQL Exception Occurred — Error Number: {ErrorNumber}, Message: {Message}",
                    sqlEx.Number, sqlEx.Message);

                return;
            }

            _logger.LogError(context.Exception,
                "Unhandled Exception Occurred: {Message}",
                context.Exception.Message);

            var defaultResponse = new ResponseModel
            {
                StatusCode = (int)ApiStatusCodeEnum.InternalServerError,
                Message = ContentLoader.ReturnLanguageData("BA516", lang),
                Data = null,
                Errors = new List<Errors>()
            };

            context.Result = new JsonResult(defaultResponse);
            context.ExceptionHandled = true;
        }
    }
}
