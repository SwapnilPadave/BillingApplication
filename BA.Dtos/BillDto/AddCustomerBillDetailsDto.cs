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
        public int NormalDays { get; set; } = 0;
        public int SundayDays { get; set; } = 0;
        public int SaturdayDays { get; set; } = 0;
        public int SpecialDays { get; set; } = 0;
        public decimal NormalDayAmount { get; set; } = 0;
        public decimal SundayAmount { get; set; } = 0;
        public decimal SaturdayAmount { get; set; } = 0;
        public decimal SpecialDayAmount { get; set; } = 0;
    }
}
