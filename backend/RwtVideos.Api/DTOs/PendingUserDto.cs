using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RwtVideos.Api.DTOs
{
    public class PendingUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}