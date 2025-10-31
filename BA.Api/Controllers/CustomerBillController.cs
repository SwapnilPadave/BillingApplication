using AutoMapper;
using BA.Api.Infra.MediatorHandlers.CustomerNewsPaperBillHandler;
using BA.Dtos.BillDto;
using BA.Service.Bill;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerBillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CustomerBillController(IBillService billService, IMapper mapper, IMediator mediator)
        {
            _billService = billService;
            _mapper = mapper;
            _mediator = mediator;
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

        [HttpPost("GetById")]
        public async Task<Dictionary<string, object>> GetCustomerBillById([FromQuery] int billId)
        {
            var result = await _billService.GetCustomerBillById(billId);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateCustomerBillDetails(int id, [FromBody] UpdateCustomerBillDetailsDto requestDto)
        {
            var result = await _billService.UpdateCustomerBillDetails(UserId, id, requestDto);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpPost("UpdateStatus")]
        public async Task<Dictionary<string, object>> UpdateBillStatusAsync(int id, bool isBillPaid)
        {
            var result = await _billService.UpdateBillStatusAsync(UserId, id, isBillPaid);
            if (result.IsSuccess)
            {
                return APIResponse("BA100", result.Data!);
            }
            return APIResponse("BA101", null!);
        }

        [HttpDelete("Delete")]
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
        public Dictionary<string, object> GetTotalMonthDaysCount([FromBody] DateRequestDto requestDto)
        {
            var result = _billService.GetSatAndSunCount(requestDto.FromDate, requestDto.ToDate, requestDto.SpecialDays);
            return APIResponse("BA100", result);
        }

        #region different ways to call async method into sync method.
        //[HttpGet("Test")]
        //public async Task<Dictionary<string, object>> Test()
        //{
        //    int id = 0;
        //    var result = _billService.GetCustomerBillById(id).GetAwaiter().GetResult();

        //    var result1 = _billService.GetCustomerBillById(id).Result;

        //    _billService.GetCustomerBillById(id).Wait();
        //    Task.WaitAll();

        //    var result2 = Task.Run(() => _billService.GetCustomerBillById(id)).GetAwaiter().GetResult();

        //    return APIResponse("", result);
        //}
        #endregion

        #region All methods are using Mediator.
        //[HttpPost("AddCustomerBillDetailsV1")]
        //public async Task<Dictionary<string, object>> AddCustomerBillDetailsV1([FromBody] AddNewsPaperBillCommand command)
        //{
        //    command.UserId = UserId;
        //    var result = await _mediator.Send(command);
        //    if (result.IsSuccess)
        //    {
        //        return APIResponse("BA100", result.Data!);
        //    }
        //    return APIResponse("BA101", null!);
        //}

        //[HttpPost("GetAllCustomerBillDetails")]
        //public async Task<Dictionary<string, object>> GetAllCustomerBillDetailsV1([FromQuery] GetAllCustomerNewsPaperBillDetailsQuery query)
        //{
        //    var result = await _mediator.Send(query);
        //    if (result.IsSuccess)
        //    {
        //        return APIResponse("BA200", result.Data!);
        //    }
        //    return APIResponse("Failed", null!);
        //}
        #endregion
    }
}
public class DateRequestDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int SpecialDays { get; set; } = 0;
}
