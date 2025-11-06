using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Entities.Bill;

namespace BA.Database.Repos.CustomerBillDetailsRepository
{
    public interface ICustomerBillDetailsRepository : IRepository<CustomerNewsPaperBillDetail>
    {
        Task<List<GetNewsPaperBillDetailsForCustomerBillDto>> GetBillDetailsAsync(int id);
    }
}
