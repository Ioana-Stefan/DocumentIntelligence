using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Services.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(IdentityUser user, IEnumerable<string> roles);

    }
}