using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Entities.Bill;

namespace BA.Database.Repos.BillRepository
{
    public interface IBillRepository : IRepository<CustomerBillDetails>
    {
        Task<IEnumerable<CustomerBillDetails>> GetAllCustomerBillsAsync();
        Task<GetCustomerBillDetailsDto> GetCustomerBillByIdAsync(int id);
    }
}
