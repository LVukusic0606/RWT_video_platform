using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RwtVideos.Api.Models;
using RwtVideos.Api.Repositories;
using RwtVideos.Api.DTOs;

namespace RwtVideos.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(string name, string email, string password)
        {
            var newUser = new User
            {
                Name = name,
                Email = email,
                IsApproved = false    
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, password);

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<AuthResponseDto?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) return null;

            var token = _tokenService.CreateToken(user);
            return new AuthResponseDto { Token = token };
        }

        public async Task<List<PendingUserDto>> GetPendingAsync()
        {
            var pendingUsers = await _userRepository.GetPendingAsync();

            var result = pendingUsers.Select(u => new PendingUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList();

            return result;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            user.IsApproved = true;
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}