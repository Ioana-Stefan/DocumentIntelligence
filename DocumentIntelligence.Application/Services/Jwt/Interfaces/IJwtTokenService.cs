using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Services.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(IdentityUser user, IEnumerable<string> roles);
        (bool IsValid, string? UserId, string? Email, IEnumerable<string> Roles) ValidateToken(string token);

    }
}