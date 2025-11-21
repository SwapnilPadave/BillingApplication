using BA.Database.Infra;
using BA.Entities.Country_State_City;

namespace BA.Database.Repos.LocationRepository
{
    public interface ICityRepository:IRepository<CityMaster>
    {
    }

    public class CityRepository : Repository<CityMaster>, ICityRepository
    {
        public CityRepository(BAContext context) : base(context)
        {
        }
    }
}
