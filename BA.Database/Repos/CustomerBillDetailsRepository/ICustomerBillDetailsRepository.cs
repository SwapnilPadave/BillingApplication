using BA.Database.Infra;
using BA.Entities.Bill;

namespace BA.Database.Repos.CustomerBillDetailsRepository
{
    public interface ICustomerBillDetailsRepository : IRepository<CustomerNewsPaperBillDetail>
    {
        Task<List<GetBillDetailsDtoForBill>> GetBillDetailsAsync(int id);
    }
}
