namespace BA.Api.Infra.Requests.NewsPaperRequests
{
    public class UpdateNewsPaperRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
