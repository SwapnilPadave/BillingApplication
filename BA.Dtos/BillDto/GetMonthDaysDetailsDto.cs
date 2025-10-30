namespace BA.Dtos.BillDto
{
    public class GetMonthDaysDetailsDto
    {
        public int Sunday { get; set; }
        public int Saturday { get; set; }
        public int SpecialDays { get; set; }
        public int NormalDays { get; set; }
    }
}
