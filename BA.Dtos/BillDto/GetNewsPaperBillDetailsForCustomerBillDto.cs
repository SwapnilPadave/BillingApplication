namespace BA.Dtos.BillDto
{
    public class GetNewsPaperBillDetailsForCustomerBillDto
    {
        public int BillId { get; set; }
        public int CustomerId { get; set; }
        public string NewsPaperName { get; set; } = string.Empty;
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
}
