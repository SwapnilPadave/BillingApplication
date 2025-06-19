using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BA.Api.Infra.Filters
{
    public class FluentValidationFilterProvider : IFilterProvider
    {
        public int Order => -1000;

        public void OnProvidersExecuting(FilterProviderContext context)
        {
            foreach (var item in context.Results)
            {
                if (item.Filter is not ControllerActionDescriptor descriptor) continue;

                var parameter = descriptor.Parameters.FirstOrDefault(p => p.ParameterType.IsClass && !p.ParameterType.IsPrimitive && p.ParameterType != typeof(string));

                if (parameter == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                var serviceProvider = context.ActionContext.HttpContext.RequestServices;
                var validator = serviceProvider.GetService(validatorType);

                if (validator != null)
                {
                    var filterType = typeof(FluentValidationActionFilter<>).MakeGenericType(parameter.ParameterType);
                    var filter = (IFilterMetadata)ActivatorUtilities.CreateInstance(serviceProvider, filterType);
                    context.Results.Add(new FilterItem(new FilterDescriptor(filter, FilterScope.Global), filter));
                }
            }
        }

        public void OnProvidersExecuted(FilterProviderContext context) { }
    }
}
