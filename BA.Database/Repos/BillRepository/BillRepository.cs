using BA.Database.Infra;
using BA.Dtos.BillDto;
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
                .Where(bill => bill.IsActive)
                .ToListAsync();
            return data;
        }

        public async Task<GetCustomerBillDetailsDto> GetCustomerBillByIdAsync(int id)
        {
            var data = await (from b in _context.CustomerBillDetails
                              join cn in _context.CustomerNewsPaperBillDetails on b.CustomerId equals cn.CustomerId
                              where b.Id == id
                              select new GetCustomerBillDetailsDto
                              {
                                  Id = b.Id,
                                  CustomerId = b.CustomerId,
                                  CustomerName = b.CustomerName,
                                  CustomerAddress = b.CustomerAddress,
                                  NewsPaperIds = b.NewsPaperIds,
                                  FromDate = b.FromDate,
                                  ToDate = b.ToDate,
                                  TotalDays = b.TotalDays,
                                  Amount = b.Amount,
                                  ServiceCharge = b.ServiceCharge,
                                  TotalAmount = b.TotalAmount,
                                  IsBillPaid = b.IsBillPaid,
                                  NewsPapersDetails = (from np in _context.NewsPaperDetails
                                                       join cnp in _context.CustomerNewsPaperBillDetails on np.Id equals cnp.NewsPaperId
                                                       where cnp.CustomerId == b.CustomerId && cnp.BillId == id
                                                       select new NewsPaperIdAndAmountDetailsDto
                                                       {
                                                           Id = cnp.NewsPaperId,
                                                           NewsPaperName = np.Name,
                                                           NormalDays = cnp.NormalDays,
                                                           SundayDays = cnp.Sundays,
                                                           SaturdayDays = cnp.Saturday,
                                                           SpecialDays = cnp.SpecialDays,
                                                           NormalDayAmount = cnp.NormalDayAmount,
                                                           SundayAmount = cnp.SundayAmount,
                                                           SaturdayAmount = cnp.SaturdayAmount,
                                                           SpecialDayAmount = cnp.SpecialDayAmount
                                                       }).ToList()
                              }).FirstOrDefaultAsync();
            return data!;
        }
    }
}
