using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Entities.Bill;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.CustomerBillDetailsRepository
{
    public class CustomerBillDetailsRepository : Repository<CustomerNewsPaperBillDetail>, ICustomerBillDetailsRepository
    {
        private readonly BAContext _context;
        public CustomerBillDetailsRepository(BAContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<GetNewsPaperBillDetailsForCustomerBillDto>> GetBillDetailsAsync(int id)
        {
            var data = await (from b in _context.CustomerNewsPaperBillDetails
                              join n in _context.NewsPaperDetails on b.NewsPaperId equals n.Id
                              where b.CustomerId == id
                              select new GetNewsPaperBillDetailsForCustomerBillDto
                              {
                                  BillId = b.Id,
                                  CustomerId = b.CustomerId,
                                  NewsPaperName = n.Name,
                                  TotalDays = b.TotalDays,
                                  NormalDays = b.NormalDays,
                                  Sundays = b.Sundays,
                                  Saturday = b.Saturday,
                                  SpecialDays = b.SpecialDays,
                                  NormalDayAmount = b.NormalDayAmount,
                                  SundayAmount = b.SundayAmount,
                                  SaturdayAmount = b.SaturdayAmount,
                                  SpecialDayAmount = b.SpecialDayAmount,
                                  TotalAmount = b.TotalAmount
                              }).ToListAsync();

            return data;
        }
    }
}
