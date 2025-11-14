using AutoMapper;
using BA.Api.Infra.Model;
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
        public async Task<ResponseModel> AddAsync([FromBody] AddCustomerRequest customerDto)
        {
            var request = _mapper.Map<AddCustomerDto>(customerDto);
            var result = await _customerService.AddCustomerAsync(UserId, request);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<ResponseModel> UpdateAsync(int id, [FromBody] UpdateCustomerRequest customerDto)
        {
            var request = _mapper.Map<UpdateCustomerDto>(customerDto);
            var result = await _customerService.UpdateCustomerAsync(UserId, id, request);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1102", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetById")]
        public async Task<ResponseModel> GetByIdAsync(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<ResponseModel> GetAllAsync()
        {
            var result = await _customerService.GetAllCustomersAsync();
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("ActivateOrDeactivate")]
        public async Task<ResponseModel> DeleteAsync(int id, bool isActive)
        {
            var result = await _customerService.DeleteCustomerAsync(UserId, id, isActive);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
