namespace BA.Dtos.BillDto
{
    public class AddCustomerBillDetailsDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public int TotalDays { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsBillPaid { get; set; }
        public List<NewsPaperIdAndAmount> NewsPaperIdAndAmount { get; set; } = new List<NewsPaperIdAndAmount>();
    }

    public class NewsPaperIdAndAmount
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
