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
            // Get the current action descriptor
            if (context.ActionContext.ActionDescriptor is not ControllerActionDescriptor descriptor)
                return;

            // Loop through the parameters of the action
            foreach (var parameter in descriptor.Parameters)
            {
                // We only care about class-type parameters (like DTOs)
                if (!parameter.ParameterType.IsClass || parameter.ParameterType == typeof(string))
                    continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
                var serviceProvider = context.ActionContext.HttpContext.RequestServices;
                var validator = serviceProvider.GetService(validatorType);

                // If no validator exists for this parameter type, skip it
                if (validator == null)
                    continue;

                // Create a matching FluentValidationActionFilter<T> dynamically
                var filterType = typeof(FluentValidationActionFilter<>).MakeGenericType(parameter.ParameterType);
                var filter = (IFilterMetadata)ActivatorUtilities.CreateInstance(serviceProvider, filterType);

                // Attach it as a global filter for this action
                context.Results.Add(new FilterItem(new FilterDescriptor(filter, FilterScope.Global), filter));
            }
        }

        public void OnProvidersExecuted(FilterProviderContext context) { }
    }
}
