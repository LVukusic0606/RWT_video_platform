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
        // DbContext je scoped, zato trebamo scope
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // 1) osiguraj da su migracije primijenjene
        await db.Database.MigrateAsync();

        // 2) pročitaj seed postavke
        var seedSection = configuration.GetSection("SeedAdmin");
        var seedEmail = seedSection["Email"];
        var seedName = seedSection["Name"];
        var seedPassword = seedSection["Password"];

        // ako nije konfigurirano, ne seedamo (da app radi i bez toga)
        if (string.IsNullOrWhiteSpace(seedEmail) ||
            string.IsNullOrWhiteSpace(seedName) ||
            string.IsNullOrWhiteSpace(seedPassword))
        {
            return;
        }

        // 3) ako admin već postoji, ne radi ništa
        var adminExists = await db.Users.AnyAsync(u => u.Email == seedEmail);
        if (adminExists) return;

        // 4) kreiraj admina
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