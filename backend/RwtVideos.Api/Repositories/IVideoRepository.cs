using RwtVideos.Api.Models;

namespace RwtVideos.Api.Repositories
{
    public interface IVideoRepository
    {
        Task AddAsync(Video video);
        Task<Video?> GetByIdAsync(int id);
        Task<List<Video>> GetActiveAsync();
        Task SaveChangesAsync();
    }
}
