using AutoMapper;
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
        public async Task<Dictionary<string, object>> GetAllAsync(CancellationToken cancellationToken)
        {
            var data = await _newsPaperService.GetNewsPapersAsync(cancellationToken);
            if (data.IsSuccess)
                return APIResponse("BA100", data.Data!);
            return APIResponse(data.Error.ErrorMsg, null!);
        }

        [HttpGet("GetById")]
        public async Task<Dictionary<string, object>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var data = await _newsPaperService.GetNewsPaperByIdAsync(id, cancellationToken);
            if (data.IsSuccess)
                return APIResponse("BA100", data.Data!);
            return APIResponse(data.Error.ErrorMsg, null!);
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddAsync([FromBody] AddNewsPaperRequest request, CancellationToken cancellationToken)
        {
            var newsPaper = _mapper.Map<AddNewsPaperDto>(request);
            var result = await _newsPaperService.AddNewsPaperAsync(UserId, newsPaper, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateAsync(int id, [FromBody] UpdateNewsPaperRequest request, CancellationToken cancellationToken)
        {
            var newsPaper = _mapper.Map<UpdateNewsPaperDto>(request);
            var result = await _newsPaperService.UpdateNewsPaperAsync(UserId, id, newsPaper, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpDelete("Delete")]
        public async Task<Dictionary<string, object>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _newsPaperService.DeleteNewsPaperAsync(UserId, id, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetEmployees")]
        [AllowAnonymous]
        public async Task<Dictionary<string, object>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var data = await _employeeService.GetEmployees();
            if (data.IsSuccess)
                return APIResponse("BA100", data.Data!);
            return APIResponse(data.Error.ErrorMsg, null!);
        }
    }
}
