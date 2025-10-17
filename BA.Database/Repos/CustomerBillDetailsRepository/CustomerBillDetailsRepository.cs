using BA.Database.Infra;
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
        public async Task<List<GetBillDetailsDtoForBill>> GetBillDetailsAsync(int id)
        {
            var data = await (from b in _context.CustomerNewsPaperBillDetails
                              join n in _context.NewsPaperDetails on b.NewsPaperId equals n.Id
                              where b.CustomerId == id
                              select new GetBillDetailsDtoForBill
                              {
                                  BillId = b.Id,
                                  CustomerId = b.CustomerId,
                                  CustomerName = n.Name,
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
public class GetBillDetailsDtoForBill
{
    public int BillId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int NormalDays { get; set; }
    public int Sundays { get; set; }
    public int Saturday { get; set; }
    public int SpecialDays { get; set; }
    public int TotalDays { get; set; }
    public decimal NormalDayAmount { get; set; }
    public decimal SundayAmount { get; set; }
    public decimal SaturdayAmount { get; set; }
    public decimal SpecialDayAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
