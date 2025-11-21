namespace BA.Dtos.BillDto
{
    public class GetBillDetailsForReportDto
    {
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? NewsPaperName { get; set; }
        public string? NewsPaperLanguage { get; set; }
        public string? BillFromDate { get; set; }
        public string? BillToDate { get; set; }
        public int? NormalDays { get; set; }
        public int? Sundays { get; set; }
        public int? Saturdays { get; set; }
        public int? SpecialDays { get; set; }
        public int? TotalDays { get; set; }
        public decimal? NormalDayAmount { get; set; }
        public decimal? SundayAmount { get; set; }
        public decimal? SaturdayAmount { get; set; }
        public decimal? SpecialDayAmount { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public bool IsBillPaid { get; set; }
    }
}
