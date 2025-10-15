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
                                  BillName = n.Name,
                                  //TotalDays = b.Quantity,
                                  //Amount = b.Price
                              }).ToListAsync();

            return data;
        }
    }
}
public class GetBillDetailsDtoForBill
{
    public int BillId { get; set; }
    public int CustomerId { get; set; }
    public string BillName { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public decimal Amount { get; set; }
}
