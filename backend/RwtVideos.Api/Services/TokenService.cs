using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var jwtSection = _configuration.GetSection("Jwt");

            var keyString = jwtSection["Key"];
            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("JWT Key is missing (Jwt:Key).");

            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            var expiresMinutes = jwtSection.GetValue<int?>("ExpiresMinutes") ?? 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("isApproved", user.IsApproved.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}