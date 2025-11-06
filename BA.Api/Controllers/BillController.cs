using BA.Service.Bill;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService = billService;
        }
        [HttpPost("GenerateAndDownloadBill")]
        public async Task<Dictionary<string, object>> GenerateAndDownloadBill(int id)
        {
            var result = await _billService.GenerateBill(UserId, id);
            if (result.IsSuccess && result.Data != null)
            {
                return APIResponse("BA507", result.Data);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
