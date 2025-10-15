using BA.Database.Infra;
using BA.Dtos.EmployeeDto;
using BA.Entities.Employees;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.EmployeeRepository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly BAContext _context;
        public EmployeeRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GetEmployeeDto>> GetEmployees()
        {
            var emp = await _context.Employees
                .Include(e => e.Department)
                .Include(e=>e.Shift)
                .Select(e => new GetEmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Gender = e.Gender,
                    Salary = e.Salary,
                    DeptId = e.DeptId,
                    DeptName = e.Department.Name,
                    ShiftId = e.ShiftId,
                    ShiftName = e.Shift.Name
                }).ToListAsync();

            return emp;
        }
    }
}
