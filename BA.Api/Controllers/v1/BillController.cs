using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.GenerateBillHandler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BillController : BaseController
    {
        private readonly IMediator _mediator;
        public BillController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("GenerateAndDownloadBill")]
        public async Task<Dictionary<string, object>> GenerateAndDownloadBill([FromQuery] GenerateAndDownloadCustomerNewsPaperBillQuery request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
