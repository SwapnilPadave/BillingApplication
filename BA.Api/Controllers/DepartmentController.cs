using BA.Api.Infra.Model;
using BA.Service.DepartmentRoleShift;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentRoleShiftService _departmentRoleShiftService;
        public DepartmentController(IDepartmentRoleShiftService departmentRoleShiftService)
        {
            _departmentRoleShiftService = departmentRoleShiftService;
        }

        [HttpGet("GetDetails")]
        public async Task<ResponseModel> GetDetails()
        {
            var result = await _departmentRoleShiftService.GetResultAsync();
            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);

        }
    }
}
