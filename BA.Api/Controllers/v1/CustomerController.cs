using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.CustomerHandler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : BaseController
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddAsync([FromBody] AddCustomerDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return APIResponse("BA1101", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateAsync([FromBody] UpdateCustomerDetailsCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return APIResponse("BA1102", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetById")]
        public async Task<Dictionary<string, object>> GetByIdAsync([FromQuery] GetCustomerDetailsByIdQuery request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<Dictionary<string, object>> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllCustomerDetailsQuery());
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Delete")]
        public async Task<Dictionary<string, object>> DeleteAsync([FromQuery] UpdateCustomerStatusQuery request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return APIResponse("BA1103", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
