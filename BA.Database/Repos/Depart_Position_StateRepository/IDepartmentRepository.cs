using BA.Database.Infra;
using BA.Entities.JobApp_Departments;

namespace BA.Database.Repos.Depart_Position_StateRepository
{
    public interface IDepartmentRepository : IRepository<JobApp_DepartmentDetailMaster>
    {
    }
    public class DepartmentRepository : Repository<JobApp_DepartmentDetailMaster>, IDepartmentRepository
    {
        public DepartmentRepository(BAContext context) : base(context)
        {
        }
    }
}
