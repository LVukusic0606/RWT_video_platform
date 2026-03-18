namespace RwtVideos.Api.Services
{
    public class VideoStorageService : IVideoStorageService
    {
        private readonly string _rootPath;

        public VideoStorageService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var configuredRoot = configuration["VideoStorage:RootPath"];
            var relativeRoot = string.IsNullOrWhiteSpace(configuredRoot) ? "Storage/Videos" : configuredRoot;
            _rootPath = Path.Combine(environment.ContentRootPath, relativeRoot);

            Directory.CreateDirectory(_rootPath);
        }

        public async Task<(string relativePath, string storedFileName)> SaveAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var dateFolder = DateTime.UtcNow.ToString("yyyy/MM");
            var storageDirectory = Path.Combine(_rootPath, dateFolder);
            Directory.CreateDirectory(storageDirectory);

            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var absolutePath = Path.Combine(storageDirectory, storedFileName);

            await using var output = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(output, cancellationToken);

            var relativePath = Path.Combine(dateFolder, storedFileName).Replace("\\", "/");
            return (relativePath, storedFileName);
        }

        public Stream OpenRead(string relativePath)
        {
            var absolutePath = BuildAbsolutePath(relativePath);
            return new FileStream(absolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public bool Exists(string relativePath)
        {
            var absolutePath = BuildAbsolutePath(relativePath);
            return File.Exists(absolutePath);
        }

        public void DeleteIfExists(string relativePath)
        {
            var absolutePath = BuildAbsolutePath(relativePath);
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }

        private string BuildAbsolutePath(string relativePath)
        {
            var safeRelativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());
            return Path.Combine(_rootPath, safeRelativePath);
        }
    }
}
