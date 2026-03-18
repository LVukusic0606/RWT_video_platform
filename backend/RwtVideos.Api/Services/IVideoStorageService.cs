namespace RwtVideos.Api.Services
{
    public interface IVideoStorageService
    {
        Task<(string relativePath, string storedFileName)> SaveAsync(IFormFile file, CancellationToken cancellationToken = default);
        Stream OpenRead(string relativePath);
        bool Exists(string relativePath);
        void DeleteIfExists(string relativePath);
    }
}
