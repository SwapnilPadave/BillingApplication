namespace BA.Dtos.BillDto
{
    public class CalculateTotalAmountDto
    {
        public List<NewsPaperIdsAndAmountDto> NewsPaperIdsAndAmountRequest { get; set; } = new List<NewsPaperIdsAndAmountDto>();
        public decimal ServiceCharge { get; set; }
    }
    public class NewsPaperIdsAndAmountDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
