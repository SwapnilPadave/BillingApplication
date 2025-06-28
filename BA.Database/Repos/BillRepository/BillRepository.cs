using BA.Database.Infra;
using BA.Entities.Bill;

namespace BA.Database.Repos.BillRepository
{
    public class BillRepository : Repository<CustomerBillDetails>, IBillRepository
    {
        public BillRepository(BAContext context) : base(context)
        {

        }
    }
}
