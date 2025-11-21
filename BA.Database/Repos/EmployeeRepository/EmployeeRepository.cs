using BA.Database.Infra;
using BA.Entities.JobApp_Employees;

namespace BA.Database.Repos.EmployeeRepository
{
    public class EmployeeRepository : Repository<JobApp_EmployeesDetails>, IEmployeeRepository
    {
        private readonly BAContext _context;
        public EmployeeRepository(BAContext context) : base(context)
        {
            _context = context;
        }
    }
}
