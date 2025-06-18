using BA.Database.Infra;
using BA.Entities.Customer;

namespace BA.Database.Repos.CustomerRepository
{
    public class CustomerDetailsRepository : Repository<CustomerDetails>, ICustomerDetailsRepository
    {
        public CustomerDetailsRepository(BAContext context) : base(context)
        {

        }
    }
}
