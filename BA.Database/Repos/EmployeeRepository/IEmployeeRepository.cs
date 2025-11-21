using BA.Database.Infra;
using BA.Dtos.EmployeeDto;
using BA.Entities.JobApp_Employees;

namespace BA.Database.Repos.EmployeeRepository
{
    public interface IEmployeeRepository : IRepository<JobApp_EmployeesDetails>
    {
    }
}
