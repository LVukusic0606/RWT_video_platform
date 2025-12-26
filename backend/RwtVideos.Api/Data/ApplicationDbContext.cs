using Microsoft.EntityFrameworkCore;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}