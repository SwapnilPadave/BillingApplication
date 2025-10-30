using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.NewsPaperBillHandler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerBillController : BaseController
    {
        private readonly IMediator _mediator;
        public CustomerBillController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> Add([FromBody] AddNewsPaperBillCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("GetById")]
        public async Task<Dictionary<string, object>> GetById([FromQuery] GetCustomerNewsPaperBillDetailsByIdQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<Dictionary<string, object>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCustomerBillDetailsQuery());
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("UpdateStatus")]
        public async Task<Dictionary<string, object>> UpdateStatus([FromQuery] UpdateCustomerNewsPapaerBillStatusQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", null!);
            }
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpDelete("Delete")]
        public async Task<Dictionary<string, object>> Delete([FromQuery] DeleteCustomerNewsPaperBillQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", null!);
            }
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetTotalMonthDaysCount")]
        public async Task<Dictionary<string, object>> GetMonthDaysCount([FromBody] GetMonthDaysCountFromDateRangeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse(result.Error.ErrorMsg, null!);
        }
    }
}
