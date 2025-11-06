using BA.Utility.Constant;
using BA.Utility.Content;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public virtual int UserId { get; set; }
        public virtual string? UserName { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string? Role { get; set; }

        protected Dictionary<string, object> APIResponse(string msgCode, object result, string languageCode = "")
        {
            var response = new Dictionary<string, object>
            {
                { Constants.RESPONSE_MESSAGE_FIELD, ContentLoader.ReturnLanguageData(msgCode, languageCode) },
                { Constants.RESPONSE_DATA_FIELD, result }
            };
            return response;
        }

        protected Dictionary<string, object> APIFailureResponse(string msgCode, object error = null, int statusCode = 400, string languageCode = "")
        {
            var response = new Dictionary<string, object>
            {
                { Constants.RESPONSE_MESSAGE_FIELD, ContentLoader.ReturnLanguageData(msgCode, languageCode) },
                { Constants.RESPONSE_DATA_FIELD, null! },
                { Constants.RESPONSE_STATUS_CODE_FIELD, statusCode },
                { Constants.RESPONSE_MODEL_STATE_ERRORS_FIELD, new List<string> { ContentLoader.ReturnLanguageData(msgCode, languageCode) } }
            };
            return response;
        }

        protected void ExtractUserContext()
        {
            if (User?.Identity?.IsAuthenticated != true)
                return;

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                UserId = userId;

            UserName = User.FindFirst("UserName")?.Value ?? string.Empty;
            IsAdmin = string.Equals(User.FindFirst("Admin")?.Value, "true", StringComparison.OrdinalIgnoreCase);
            IsActive = string.Equals(User.FindFirst("IsActive")?.Value, "true", StringComparison.OrdinalIgnoreCase);
            Role = User.FindFirst("Role")?.Value ?? string.Empty;
        }
    }
}
