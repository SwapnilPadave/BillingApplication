using AutoMapper;
using BA.Dtos.BillDto;
using BA.Service.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerBillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IMapper _mapper;
        public CustomerBillController(IBillService billService, IMapper mapper)
        {
            _billService = billService;
            _mapper = mapper;
        }
        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddCustomerBillDetails([FromBody] AddCustomerBillDetailsDto requestDto)
        {
            var result = await _billService.AddCustomerBillDetails(UserId, requestDto);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpGet("GetAll")]
        public async Task<Dictionary<string, object>> GetAllCustomerBills()
        {
            var result = await _billService.GetAllCustomerBills();
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpGet("GetById")]
        [AllowAnonymous]
        public async Task<Dictionary<string, object>> GetCustomerBillById(int id)
        {
            var result = await _billService.GetCustomerBillById(id);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<Dictionary<string, object>> UpdateCustomerBillDetails(int id, [FromBody] UpdateCustomerBillDetailsDto requestDto)
        {
            var result = await _billService.UpdateCustomerBillDetails(UserId, id, requestDto);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpDelete("Delete")]
        [AllowAnonymous]
        public async Task<Dictionary<string, object>> DeleteCustomerBill(int id)
        {
            var result = await _billService.DeleteCustomerBill(UserId, id);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("GetTotalMonthDaysCount")]
        [AllowAnonymous]
        public Dictionary<string, object> GetTotalMonthDaysCount([FromBody] DateRequestDto requestDto)
        {
            var result = _billService.GetSatAndSunCount(requestDto.FromDate, requestDto.ToDate, requestDto.SpecialDays);
            return APIResponse("BA100", result);
        }
    }
}
public class DateRequestDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int SpecialDays { get; set; } = 0;
}
