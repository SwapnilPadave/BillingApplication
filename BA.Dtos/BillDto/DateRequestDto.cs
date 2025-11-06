namespace BA.Dtos.BillDto
{
    public class DateRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int SpecialDays { get; set; } = 0;
    }
}
