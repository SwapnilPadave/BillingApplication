using BA.Database.Infra;
using BA.Entities.JobApp_Shifts;

namespace BA.Database.Repos.Depart_Position_StateRepository
{
    public interface IShiftRepository : IRepository<JobApp_ShiftDetailsMaster>
    {
    }
    public class ShiftRepository : Repository<JobApp_ShiftDetailsMaster>, IShiftRepository
    {
        public ShiftRepository(BAContext context) : base(context)
        {
        }
    }
}
