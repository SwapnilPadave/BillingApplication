namespace BA.Entities.VideoStream
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UploadedUserId { get; set; }
        public string FolderRelativePath { get; set; } = string.Empty;
        public string ThumbnailRelativePath { get; set; } = string.Empty;
        public string Status { get; set; } = "Uploaded";
        public long Views { get; set; } = 0;
        public TimeSpan? Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<VideoFormat> Formats { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
    }
}
