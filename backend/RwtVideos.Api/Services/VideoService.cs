using RwtVideos.Api.DTOs;
using RwtVideos.Api.Models;
using RwtVideos.Api.Repositories;

namespace RwtVideos.Api.Services
{
    public class VideoService : IVideoService
    {
        private static readonly string[] AllowedExtensions = [".mp4", ".webm", ".mov", ".m4v"];

        private readonly IVideoRepository _videoRepository;
        private readonly IVideoStorageService _videoStorageService;
        private readonly long _maxFileSizeBytes;

        public VideoService(
            IVideoRepository videoRepository,
            IVideoStorageService videoStorageService,
            IConfiguration configuration)
        {
            _videoRepository = videoRepository;
            _videoStorageService = videoStorageService;

            var maxSizeMb = configuration.GetValue<int?>("VideoStorage:MaxFileSizeMB") ?? 500;
            _maxFileSizeBytes = maxSizeMb * 1024L * 1024L;
        }

        public async Task<int> UploadAsync(UploadVideoDto dto, int uploadedByUserId, CancellationToken cancellationToken = default)
        {
            ValidateUpload(dto);

            var (relativePath, storedFileName) = await _videoStorageService.SaveAsync(dto.File, cancellationToken);

            try
            {
                var video = new Video
                {
                    Title = dto.Title.Trim(),
                    Description = dto.Description?.Trim() ?? string.Empty,
                    OriginalFileName = dto.File.FileName,
                    StoredFileName = storedFileName,
                    RelativePath = relativePath,
                    ContentType = string.IsNullOrWhiteSpace(dto.File.ContentType) ? "application/octet-stream" : dto.File.ContentType,
                    FileSizeBytes = dto.File.Length,
                    UploadedByUserId = uploadedByUserId
                };

                await _videoRepository.AddAsync(video);
                await _videoRepository.SaveChangesAsync();

                return video.Id;
            }
            catch
            {
                _videoStorageService.DeleteIfExists(relativePath);
                throw;
            }
        }

        public async Task<List<VideoSummaryDto>> GetAllActiveAsync()
        {
            var videos = await _videoRepository.GetActiveAsync();
            return videos.Select(v => new VideoSummaryDto
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Description,
                FileSizeBytes = v.FileSizeBytes,
                UploadedAtUtc = v.UploadedAtUtc
            }).ToList();
        }

        public async Task<VideoFileDto?> GetVideoFileAsync(int id)
        {
            var video = await _videoRepository.GetByIdAsync(id);
            if (video == null)
            {
                return null;
            }

            if (!_videoStorageService.Exists(video.RelativePath))
            {
                return null;
            }

            return new VideoFileDto
            {
                Stream = _videoStorageService.OpenRead(video.RelativePath),
                ContentType = video.ContentType,
                DownloadFileName = video.OriginalFileName
            };
        }

        private void ValidateUpload(UploadVideoDto dto)
        {
            if (dto.File == null || dto.File.Length <= 0)
            {
                throw new ArgumentException("Video file is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title is required.");
            }

            if (dto.File.Length > _maxFileSizeBytes)
            {
                throw new ArgumentException($"Video exceeds the allowed size limit ({_maxFileSizeBytes / (1024 * 1024)} MB).");
            }

            var extension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Unsupported video file type.");
            }
        }
    }
}
