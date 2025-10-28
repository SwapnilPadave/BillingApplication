namespace BA.Dtos.BillDto
{
    public class GetCustomerBillDetailsDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
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
        public List<NewsPaperIdAndAmountDetailsDto> NewsPapersDetails { get; set; } = new List<NewsPaperIdAndAmountDetailsDto>();
    }

    public class NewsPaperIdAndAmountDetailsDto
    {
        public int Id { get; set; }
        public string NewsPaperName { get; set; } = string.Empty;
        public int NormalDays { get; set; } = 0;
        public int SundayDays { get; set; } = 0;
        public int SaturdayDays { get; set; } = 0;
        public int SpecialDays { get; set; } = 0;
        public decimal NormalDayAmount { get; set; } = 0;
        public decimal SundayAmount { get; set; } = 0;
        public decimal SaturdayAmount { get; set; } = 0;
        public decimal SpecialDayAmount { get; set; } = 0;
    }

    public class NewsPaperDetailsDto
    {
        public int NewsPaperId { get; set; }
        public string NewsPaperName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
