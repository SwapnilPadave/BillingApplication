using BA.Database.Infra;
using BA.Entities.Country_State_City;

namespace BA.Database.Repos.LocationRepository
{
    public interface ICountryRepository : IRepository<CountryMaster>
    {
    }

    public class CountryRepository : Repository<CountryMaster>, ICountryRepository
    {
        public CountryRepository(BAContext context) : base(context)
        {
        }
    }
}
