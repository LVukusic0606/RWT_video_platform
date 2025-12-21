namespace RwtVideos.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public int Age { get; set; }
    }
}