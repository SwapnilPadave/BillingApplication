using AutoMapper;
using BA.Api.Infra.Requests.CustomerRequest;
using BA.Dtos.CustomerDto;
using BA.Service.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CustomerController(
            ICustomerService customerService,
            IMapper mapper,
            IMediator mediator)
        {
            _customerService = customerService;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddAsync([FromBody] AddCustomerRequest customerDto)
        {
            var request = _mapper.Map<AddCustomerDto>(customerDto);
            var result = await _customerService.AddCustomerAsync(UserId, request);
            if (result.IsSuccess)
            {
                return APIResponse("BA1100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateAsync(int id, [FromBody] UpdateCustomerRequest customerDto)
        {
            var request = _mapper.Map<UpdateCustomerDto>(customerDto);
            var result = await _customerService.UpdateCustomerAsync(UserId, id, request);
            if (result.IsSuccess)
            {
                return APIResponse("BA1102", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetById")]
        public async Task<Dictionary<string, object>> GetByIdAsync(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<Dictionary<string, object>> GetAllAsync()
        {
            var result = await _customerService.GetAllCustomersAsync();
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Delete")]
        public async Task<Dictionary<string, object>> DeleteAsync(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(UserId, id);
            if (result.IsSuccess)
            {
                return APIResponse("BA1104", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
