using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RwtVideos.Api.Models;
using RwtVideos.Api.Repositories;
using RwtVideos.Api.DTOs;

namespace RwtVideos.Api.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string name, string email, string password);
        Task<AuthResponseDto?> LoginAsync(string email, string password); 
        Task<List<PendingUserDto>> GetPendingAsync();
        Task<bool> ApproveAsync(int id);
    }
}