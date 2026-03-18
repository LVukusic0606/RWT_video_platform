namespace RwtVideos.Api.Models
{
    public class Video
    {
        public int Id { get; private set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFileName { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public int UploadedByUserId { get; set; }
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
