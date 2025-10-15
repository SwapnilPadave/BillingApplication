namespace BA.Api.Infra.Requests.BillRequest
{
    public class CalculateTotalAmountRequest
    {
        public List<NewsPaperIdsAndAmountRequest> NewsPaperIdsAndAmountRequest { get; set; } = new List<NewsPaperIdsAndAmountRequest>();
        public decimal ServiceCharge { get; set; }
    }
    public class NewsPaperIdsAndAmountRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
}
