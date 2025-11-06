using BA.Entities.Bill;

namespace BA.Dtos.BillDto
{
    public class GetAllBilDetailsWithNewsPaperDto
    {
        public CustomerBillDetails CustomerBillDetails { get; set; } = new CustomerBillDetails();
        public List<GetNewsPaperBillDetailsForCustomerBillDto> BillDetails { get; set; } = new List<GetNewsPaperBillDetailsForCustomerBillDto>();
    }
}
