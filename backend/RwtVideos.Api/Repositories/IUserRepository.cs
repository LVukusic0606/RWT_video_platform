using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetPendingAsync();
        Task SaveChangesAsync();
    }
}