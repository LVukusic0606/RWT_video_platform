namespace RwtVideos.Api.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsApproved { get; set; } = false;
    }
}