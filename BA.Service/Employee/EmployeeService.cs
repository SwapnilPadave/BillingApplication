using BA.Database.Infra;
using BA.Dtos.EmployeeDto;
using BA.Utility.Result;
using Microsoft.AspNetCore.Http;

namespace BA.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> GetEmployees()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetEmployees();
            return Result.Success(employees);
        }
    }
}
