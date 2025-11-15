namespace DocumentIntelligence.Application.Services.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Microsoft.AspNetCore.Identity.IdentityUser user, IEnumerable<string> roles);
    }
}