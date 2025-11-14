using AutoMapper;
using BA.Api.Infra.Model;
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
        public async Task<ResponseModel> AddCustomerBillDetails([FromBody] AddCustomerBillDetailsDto requestDto)
        {
            var result = await _billService.AddCustomerBillDetails(UserId, requestDto);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1205", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<ResponseModel> GetAllCustomerBills()
        {
            var result = await _billService.GetAllCustomerBills();
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<ResponseModel> GetCustomerBillById([FromQuery] int billId)
        {
            var result = await _billService.GetCustomerBillById(billId);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<ResponseModel> UpdateCustomerBillDetails(int id, [FromBody] UpdateCustomerBillDetailsDto requestDto)
        {
            var result = await _billService.UpdateCustomerBillDetails(UserId, id, requestDto);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1208", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("UpdateStatus")]
        public async Task<ResponseModel> UpdateBillStatusAsync(int id, bool isBillPaid)
        {
            var result = await _billService.UpdateBillStatusAsync(UserId, id, isBillPaid);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1206", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpDelete("Delete")]
        public async Task<ResponseModel> DeleteCustomerBill(int id)
        {
            var result = await _billService.DeleteCustomerBill(UserId, id);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1203", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetTotalMonthDaysCount")]
        public ResponseModel GetTotalMonthDaysCount([FromBody] DateRequestDto requestDto)
        {
            var result = _billService.GetSatAndSunCount(requestDto.FromDate, requestDto.ToDate, requestDto.SpecialDays);
            return APISuccessResponse("BA100", result);
        }

        #region different ways to call async method into sync method.
        //[HttpGet("Test")]
        //public async Task<ResponseModel> Test()
        //{
        //    int id = 0;
        //    var result = _billService.GetCustomerBillById(id).GetAwaiter().GetResult();

        //    var result1 = _billService.GetCustomerBillById(id).Result;

        //    _billService.GetCustomerBillById(id).Wait();
        //    Task.WaitAll();

        //    var result2 = Task.Run(() => _billService.GetCustomerBillById(id)).GetAwaiter().GetResult();

        //    return APISuccessResponse("", result);
        //}
        #endregion

        #region All methods are using Mediator.
        //[HttpPost("AddCustomerBillDetailsV1")]
        //public async Task<ResponseModel> AddCustomerBillDetailsV1([FromBody] AddNewsPaperBillCommand command)
        //{
        //    command.UserId = UserId;
        //    var result = await _mediator.Send(command);
        //    if (result.IsSuccess)
        //    {
        //        return APISuccessResponse("BA100", result.Data!);
        //    }
        //    return APISuccessResponse("BA101", null!);
        //}

        //[HttpPost("GetAllCustomerBillDetails")]
        //public async Task<ResponseModel> GetAllCustomerBillDetailsV1([FromQuery] GetAllCustomerNewsPaperBillDetailsQuery query)
        //{
        //    var result = await _mediator.Send(query);
        //    if (result.IsSuccess)
        //    {
        //        return APISuccessResponse("BA200", result.Data!);
        //    }
        //    return APISuccessResponse("Failed", null!);
        //}
        #endregion
    }
}
