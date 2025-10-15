using BA.Database.Infra;
using BA.Dtos.EmployeeDto;
using BA.Entities.Employees;

namespace BA.Database.Repos.EmployeeRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<List<GetEmployeeDto>> GetEmployees();
    }
}
