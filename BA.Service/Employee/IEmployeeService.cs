using BA.Utility.Result;

namespace BA.Service.Employee
{
    public interface IEmployeeService
    {
        Task<Result> GetEmployees();
    }
}
