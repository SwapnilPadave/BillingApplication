using BA.Api.Infra.Model;
using BA.Api.Infra.Requests.CommonRequest;
using BA.Service.Bill;
using BA.Utility.Common_Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Authorize]
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
        public async Task<ResponseModel> GenerateAndDownloadBill(int id)
        {
            var result = await _billService.GenerateBill(UserId, id);
            if (result.IsSuccess && result.Data != null)
            {
                return APISuccessResponse("BA507", result.Data);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("ExportToExcel")]
        public async Task<ResponseModel> ExportToExcelAsync([FromQuery] BillReportFilterRequest request)
        {
            var isBothDateSelected = CommonValidatorMethods.IsBothDatesProvided(request.FromDate, request.ToDate);
            if (isBothDateSelected != "Success.")
                return APIFailureResponse(isBothDateSelected, null!);

            var result = await _billService.ExportToExcelAsync(request?.FromDate, request?.ToDate, request?.CustomerId);
            if (result.IsSuccess && result.Data != null)
            {
                return APISuccessResponse("BA601", new 
                { 
                    FileName = "BillDetailsReport",
                    FileExtension = ".xlsx",
                    Base64Date = result.Data 
                });
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
