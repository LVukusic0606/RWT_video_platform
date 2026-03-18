using Microsoft.EntityFrameworkCore;
using RwtVideos.Api.Data;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly ApplicationDbContext _context;

        public VideoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Video video)
        {
            await _context.Videos.AddAsync(video);
        }

        public async Task<Video?> GetByIdAsync(int id)
        {
            return await _context.Videos.FirstOrDefaultAsync(v => v.Id == id && v.IsActive);
        }

        public async Task<List<Video>> GetActiveAsync()
        {
            return await _context.Videos
                .Where(v => v.IsActive)
                .OrderByDescending(v => v.UploadedAtUtc)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
