using AutoMapper;
using BA.Api.Infra.Model;
using BA.Api.Infra.Requests.NewsPaperRequests;
using BA.Dtos.NewsPaperDto;
using BA.Service.Employee;
using BA.Service.NewsPaper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsPaperController : BaseController
    {
        private readonly INewsPaperService _newsPaperService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public NewsPaperController(INewsPaperService newsPaperService, IMapper mapper, IEmployeeService employeeService)
        {
            _newsPaperService = newsPaperService;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet("GetAll")]
        public async Task<ResponseModel> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _newsPaperService.GetNewsPapersAsync(cancellationToken);
            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<ResponseModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _newsPaperService.GetNewsPaperByIdAsync(id, cancellationToken);
            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Add")]
        public async Task<ResponseModel> AddAsync([FromBody] AddNewsPaperRequest request, CancellationToken cancellationToken)
        {
            var newsPaper = _mapper.Map<AddNewsPaperDto>(request);
            var result = await _newsPaperService.AddNewsPaperAsync(UserId, newsPaper, cancellationToken);
            if (result.IsSuccess)
                return APISuccessResponse("BA701", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<ResponseModel> UpdateAsync(int id, [FromBody] UpdateNewsPaperRequest request, CancellationToken cancellationToken)
        {
            var newsPaper = _mapper.Map<UpdateNewsPaperDto>(request);
            var result = await _newsPaperService.UpdateNewsPaperAsync(UserId, id, newsPaper, cancellationToken);
            if (result.IsSuccess)
                return APISuccessResponse("BA703", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        //[HttpPost("Delete")]
        //public async Task<ResponseModel> DeleteAsync(int id, CancellationToken cancellationToken)
        //{
        //    var result = await _newsPaperService.DeleteNewsPaperAsync(UserId, id, cancellationToken);
        //    if (result.IsSuccess)
        //        return APISuccessResponse("BA705", result.Data!);
        //    return APIFailureResponse(result.Error.ErrorMsg, null!);
        //}

        [HttpPost("ActivateOrDeactivate")]
        public async Task<ResponseModel> ActivateOrDeactivateAsync(int id, bool isActive, CancellationToken cancellationToken)
        {
            var result = await _newsPaperService.ActivateOrDeactivateAsync(UserId, id,isActive, cancellationToken);
            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetEmployees")]
        [AllowAnonymous]
        public async Task<ResponseModel> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var result = await _employeeService.GetEmployees();
            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
