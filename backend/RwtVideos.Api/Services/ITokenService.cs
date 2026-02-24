using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}