using BA.Api.Infra.Model;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : BaseController
    {
        [HttpPost("DownloadExcelFile")]
        public IActionResult DownloadExcelFile([FromBody] RequestHelperModel request)
        {
            if (string.IsNullOrEmpty(request.Base64String))
                return BadRequest("Base64 string is null or empty.");

            var cleanedBase64 = request.Base64String.Contains(",")
                        ? request.Base64String.Split(',')[1]
                        : request.Base64String;

            byte[] fileBytes;

            try
            {
                fileBytes = Convert.FromBase64String(cleanedBase64);
            }
            catch
            {
                return BadRequest("Invalid Base64 string.");
            }

            string fileName = "Report.xlsx";

            return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}