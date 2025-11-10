namespace BA.Entities.VideoStream
{
    public class VideoFormat
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public Video? Video { get; set; }
        public string Resolution { get; set; } = string.Empty;
        public string FileRelativePath { get; set; } = string.Empty;
    }
}
