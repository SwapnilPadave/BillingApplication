using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler;
using BA.Api.Infra.Model;
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
        public async Task<ResponseModel> Add([FromBody] AddNewsPaperBillCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1205", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<ResponseModel> GetById([FromQuery] GetCustomerNewsPaperBillDetailsByIdQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<ResponseModel> GetAll()
        {
            var result = await _mediator.Send(new GetAllCustomerBillDetailsQuery());
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("UpdateStatus")]
        public async Task<ResponseModel> UpdateStatus([FromQuery] UpdateCustomerNewsPapaerBillStatusQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1206", null!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpDelete("Delete")]
        public async Task<ResponseModel> Delete([FromQuery] DeleteCustomerNewsPaperBillQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1207", null!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetTotalMonthDaysCount")]
        public async Task<ResponseModel> GetMonthDaysCount([FromBody] GetMonthDaysCountFromDateRangeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
