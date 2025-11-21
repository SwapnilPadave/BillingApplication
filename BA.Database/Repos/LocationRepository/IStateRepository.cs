using BA.Database.Infra;
using BA.Entities.Country_State_City;

namespace BA.Database.Repos.LocationRepository
{
    public interface IStateRepository : IRepository<StateMaster>
    {
    }

    public class StateRepository : Repository<StateMaster>, IStateRepository
    {
        public StateRepository(BAContext context) : base(context)
        {
        }
    }
}
