﻿using BA.Api.Infra.Model;
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
            if (context.Controller is Controllers.BaseController baseController)
            {
                var user = context.HttpContext.User;

                if (user?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = user.FindFirst("UserId")?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                        baseController.UserId = userId;

                    baseController.UserName = user.FindFirst("UserName")?.Value ?? string.Empty;

                    baseController.IsAdmin = string.Equals(
                        user.FindFirst("Admin")?.Value, "true", StringComparison.OrdinalIgnoreCase);

                    baseController.IsActive = string.Equals(
                        user.FindFirst("IsActive")?.Value, "true", StringComparison.OrdinalIgnoreCase);
                }
            }

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                context.Result = new JsonResult(new Dictionary<string, object>
                                 {
                                     { Constants.RESPONSE_MESSAGE_FIELD, "Validation failed" },
                                     { Constants.RESPONSE_MODEL_STATE_ERRORS_FIELD, errors }
                                 })
                {
                    StatusCode = (int)HttpStatusCode.PreconditionFailed
                };
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;

            if (context.Result!.GetType() == typeof(FileContentResult))
                return;

            var result = context.Result;
            var responseObj = new ResponseModel
            {
                Message = string.Empty,
                StatusCode = 200
            };

            switch (result)
            {
                case OkObjectResult okresult:
                    responseObj.Data = okresult.Value!;
                    break;
                case ObjectResult objectResult:
                    var data = (Dictionary<string, object>)(objectResult.Value!);

                    var errorType = data.Keys.LastOrDefault();

                    switch (errorType)
                    {
                        case "Data":
                            responseObj.StatusCode = (int)HttpStatusCode.OK;
                            responseObj.Message = ContentLoader.ReturnLanguageData(Convert.ToString(data[Constants.RESPONSE_MESSAGE_FIELD])!, "")!;
                            responseObj.Data = data[Constants.RESPONSE_DATA_FIELD];
                            break;

                        case "ModelStateErrors":
                            responseObj.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                            responseObj.Message = ContentLoader.ReturnLanguageData(Convert.ToString(data[Constants.RESPONSE_MESSAGE_FIELD])!, "")!;
                            responseObj.Data = data[Constants.RESPONSE_MODEL_STATE_ERRORS_FIELD];
                            break;

                        default:
                            responseObj.StatusCode = (int)HttpStatusCode.NotFound;
                            responseObj.Message = "";
                            responseObj.Data = null;
                            break;
                    }


                    break;
                case JsonResult json:
                    responseObj.Data = json.Value;
                    break;
                case OkResult _:
                case EmptyResult _:
                    responseObj.Data = null;
                    break;
                case UnauthorizedResult _:
                    responseObj.StatusCode = (int)HttpStatusCode.Unauthorized;
                    responseObj.Message = ContentLoader.ReturnLanguageData("MSG101", "");
                    responseObj.Data = null;
                    break;
                default:
                    responseObj.Data = result;
                    break;
            }

            context.Result = new JsonResult(responseObj);
        }       
    }
}
