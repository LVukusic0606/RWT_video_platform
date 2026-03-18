namespace RwtVideos.Api.DTOs
{
    public class VideoSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAtUtc { get; set; }
    }
}
