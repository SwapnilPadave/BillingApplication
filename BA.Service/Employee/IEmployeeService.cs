using BA.Dtos.EmployeeDto;
using BA.Utility.Result;

namespace BA.Service.Employee
{
    public interface IEmployeeService
    {
        Task<Result> AddEmployeeDetailsAsync(AddEmployeeDetailsDto request);
        Task<Result> GetAllEmployeesAsync();
        Task<Result> GetEmployeeByIdAsync(int id);
        Task<Result> UpdateEmployeeDetailsAsync(UpdateEmployeeDetailsDto request);
        Task<Result> ActivateOrDeactivate(int id, bool isActivate);
        Task<Result> ExportToExcelAsync(string? fromDate, string? toDate);
        Task<Result> BulkUploadEmployeeDetails(string xmlData);
    }
}
