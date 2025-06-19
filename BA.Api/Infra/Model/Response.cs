namespace BA.Api.Infra.Model
{
    public class Response
    {
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
        public List<string> errors { get; set; } = [];
    }
}
