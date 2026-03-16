using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();

        var seedSection = configuration.GetSection("SeedAdmin");
        var seedEmail = seedSection["Email"];
        var seedName = seedSection["Name"];
        var seedPassword = seedSection["Password"];

        if (string.IsNullOrWhiteSpace(seedEmail) ||
            string.IsNullOrWhiteSpace(seedName) ||
            string.IsNullOrWhiteSpace(seedPassword))
        {
            return;
        }

        var adminExists = await db.Users.AnyAsync(u => u.Email == seedEmail);
        if (adminExists) return;

        var admin = new User
        {
            Name = seedName,
            Email = seedEmail,
            IsApproved = true,
            Role = "Admin"
        };

        var hasher = new PasswordHasher<User>();
        admin.PasswordHash = hasher.HashPassword(admin, seedPassword);

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}