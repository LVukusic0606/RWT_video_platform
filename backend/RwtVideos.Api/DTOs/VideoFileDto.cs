namespace RwtVideos.Api.DTOs
{
    public class VideoFileDto
    {
        public required Stream Stream { get; init; }
        public required string ContentType { get; init; }
        public required string DownloadFileName { get; init; }
    }
}
