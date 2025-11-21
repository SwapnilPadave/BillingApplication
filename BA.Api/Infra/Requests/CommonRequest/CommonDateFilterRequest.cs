namespace BA.Api.Infra.Requests.CommonRequest
{
    public class CommonDateFilterRequest
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class BillReportFilterRequest : CommonDateFilterRequest
    {
        public int? CustomerId { get; set; }
    }
}
