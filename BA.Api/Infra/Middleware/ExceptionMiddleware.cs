using BA.Api.Infra.Model;
using BA.Utility.Constant;
using BA.Utility.Content;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BA.Api.Infra.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ResponseModel
            {
                Errors = new()
            };

            switch (exception)
            {
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ContentLoader.ReturnLanguageData(
                        "BA511", Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])
                    );
                    break;

                case ValidationException valEx:
                    response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                    response.Message = ContentLoader.ReturnLanguageData(
                        valEx.Message, Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])
                    );
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = ContentLoader.ReturnLanguageData(
                        "BA501", Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD])
                    );
                    break;
            }

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(response.ToString());
        }
    }
}
