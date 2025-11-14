using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BA.Api.Infra.Model;
using BA.Utility.Constant;
using BA.Utility.Content;

namespace BA.Api.Infra.Filters
{
    public class FluentValidationActionFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public FluentValidationActionFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

            if (model == null)
            {
                context.Result = new JsonResult(new ResponseModel
                {
                    StatusCode = 412,
                    Message = ContentLoader.ReturnLanguageData("BA512",
                              Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                    Data = null,
                    Errors = new List<Errors>
                    {
                        new Errors
                        {
                            PropertyName = "Request",
                            ErrorMessages = new[] { "Invalid request body or missing data." }
                        }
                    }
                });

                return;
            }

            var result = await _validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                var errorList = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .Select(group => new Errors
                    {
                        PropertyName = group.Key,
                        ErrorMessages = group.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToList();

                context.Result = new JsonResult(new ResponseModel
                {
                    StatusCode = 412,
                    Message = ContentLoader.ReturnLanguageData("BA510"
                              , Convert.ToString(context.HttpContext.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])),
                    Data = null,
                    Errors = errorList
                });

                return;
            }

            await next();
        }
    }
}
