using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                context.Result = new BadRequestObjectResult(new
                {
                    statusCode = 400,
                    errors = new[] { "Invalid request body" }
                });
                return;
            }

            var result = await _validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    statusCode = 400,
                    errors = result.Errors.Select(e => e.ErrorMessage).ToList()
                });
                return;
            }

            await next();
        }
    }
}
