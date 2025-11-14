using BA.Api.Infra.Model;
using BA.Utility.Constant;
using BA.Utility.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BA.Api.Infra.Filters
{
    public class ActionFilter : IActionFilter
    {
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
                    StatusCode = (int)HttpStatusCode.PreconditionFailed, // 412 for validation failure
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
                        StatusCode = 200,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = okResult.Value,
                        Errors = new List<Errors>()
                    };
                    break;

                case ObjectResult objectResult:
                    response = new ResponseModel
                    {
                        StatusCode = 200,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = objectResult.Value,
                        Errors = new List<Errors>()
                    };
                    break;

                case OkResult:
                case EmptyResult:
                    response = new ResponseModel
                    {
                        StatusCode = 200,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = null,
                        Errors = new List<Errors>()
                    };
                    break;

                case UnauthorizedResult:
                    response = new ResponseModel
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = ContentLoader.ReturnLanguageData("BA511", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = null,
                        Errors = new List<Errors>()
                    };
                    break;

                default:
                    response = new ResponseModel
                    {
                        StatusCode = 200,
                        Message = ContentLoader.ReturnLanguageData("BA100", Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                        Data = context.Result,
                        Errors = new List<Errors>()
                    };
                    break;
            }

            context.Result = new JsonResult(response);
        }
    }
}
