using RwtVideos.Api.DTOs;

namespace RwtVideos.Api.Services
{
    public interface IVideoService
    {
        Task<int> UploadAsync(UploadVideoDto dto, int uploadedByUserId, CancellationToken cancellationToken = default);
        Task<List<VideoSummaryDto>> GetAllActiveAsync();
        Task<VideoFileDto?> GetVideoFileAsync(int id);
    }
}
