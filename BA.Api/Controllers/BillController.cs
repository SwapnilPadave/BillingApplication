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
            var data = await _billService.GenerateBill(id);

            if (data == null || data.Length == 0)
                return APIResponse("BA107", null!);

            var base64String = Convert.ToBase64String(data);

            return APIResponse("BA106", data);
        }
    }
}
