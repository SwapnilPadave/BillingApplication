namespace BA.Entities.VideoStream
{
    public class Comment
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
