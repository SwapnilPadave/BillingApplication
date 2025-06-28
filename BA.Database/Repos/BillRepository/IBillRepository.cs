using BA.Database.Infra;
using BA.Entities.Bill;

namespace BA.Database.Repos.BillRepository
{
    public interface IBillRepository : IRepository<CustomerBillDetails>
    {
    }
}
