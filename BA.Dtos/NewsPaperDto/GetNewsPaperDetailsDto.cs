namespace BA.Dtos.NewsPaperDto
{
    public class GetNewsPaperDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
