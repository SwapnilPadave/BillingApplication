using BA.Utility.Constant;
using BA.Utility.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Dictionary<string, object> APIResponse(string msgCode, object result, string languageCode = "")
        {
            var response = new Dictionary<string, object>
            {
                { Constants.RESPONSE_MESSAGE_FIELD, ContentLoader.ReturnLanguageData(msgCode, languageCode) },
                { Constants.RESPONSE_DATA_FIELD, result }
            };
            return response;
        }
    }
}
