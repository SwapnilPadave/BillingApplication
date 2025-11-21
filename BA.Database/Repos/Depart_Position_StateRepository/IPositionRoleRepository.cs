using BA.Database.Infra;
using BA.Entities.JobApp_PositionRole;

namespace BA.Database.Repos.Depart_Position_StateRepository
{
    public interface IPositionRoleRepository : IRepository<JobApp_PositionRoleDetailMaster>
    {
    }
    public class PositionRoleRepository : Repository<JobApp_PositionRoleDetailMaster>, IPositionRoleRepository
    {
        public PositionRoleRepository(BAContext context) : base(context)
        {
        }
    }
}
