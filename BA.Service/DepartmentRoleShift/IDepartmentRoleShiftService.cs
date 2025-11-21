using BA.Database;
using BA.Dtos.DPSDto;
using BA.Utility.Result;
using Dapper;

namespace BA.Service.DepartmentRoleShift
{
    public interface IDepartmentRoleShiftService
    {
        Task<Result> GetResultAsync();
    }

    public class DepartmentRoleShiftService : IDepartmentRoleShiftService
    {
        private readonly DapperServiceHelper _dapper;
        public DepartmentRoleShiftService(DapperServiceHelper dapper)
        {
            _dapper = dapper;
        }

        public async Task<Result> GetResultAsync()
        {
            try
            {
                var resultSet = new GetDepartmentRoleShiftDetailsDto();

                var data = await _dapper.QueryMultipleAsync("Usp_GetDepartmentPositionShiftDetails", new DynamicParameters(), multi =>
                {
                    var departments = multi.Read<GetDepartmentDto>().ToList();
                    var positionRoles = multi.Read<GetPositionRoleDto>().ToList();
                    var shifts = multi.Read<GetShiftDto>().ToList();

                    resultSet.Departments = departments;
                    resultSet.PositionRoles = positionRoles;
                    resultSet.Shifts = shifts;

                    return resultSet;
                });

                return Result.Success(resultSet);
            }
            catch (Exception)
            {
                return Result.Failure(new Error("BA501"));
            }
        }
    }
}
