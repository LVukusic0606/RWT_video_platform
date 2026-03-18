namespace RwtVideos.Api.DTOs
{
    public class UploadVideoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }
}
