using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClemBot.Api.Core.Security.JwtToken
{
    public interface IJwtAuthManager
    {
        public string GenerateToken(IEnumerable<Claim> claims, DateTime now);
    }
}