namespace BA.Dtos.BillDto
{
    public class GetCustomerBillDetailsDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string NewsPaperIds { get; set; } = string.Empty;
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public int TotalDays { get; set; }

        public decimal Amount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsBillPaid { get; set; }
        public List<NewsPaperDetailsDto> NewsPapersDetails { get; set; } = new List<NewsPaperDetailsDto>();
    }

    public class NewsPaperDetailsDto
    {
        public int NewsPaperId { get; set; }
        public string NewsPaperName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
