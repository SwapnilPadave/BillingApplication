using BA.Database.Infra;
using BA.Entities.Bill;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.BillRepository
{
    public class BillRepository : Repository<CustomerBillDetails>, IBillRepository
    {
        private readonly BAContext _context;
        public BillRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerBillDetails>> GetAllCustomerBillsAsync()
        {
            var data = await _context.CustomerBillDetails
                .Where(bill => bill.IsActive && bill.IsBillPaid)
                .ToListAsync();
            return data;
        }

        public async Task<CustomerBillDetails> GetCustomerBillByIdAsync(int id)
        {
            var data = await (from b in _context.CustomerBillDetails
                              where b.Id == id && b.IsActive
                              select b).FirstOrDefaultAsync();
            return data!;
        }
    }
}
