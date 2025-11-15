using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Services.Auth.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshTokenAsync(IdentityUser user);
        Task<IdentityUser> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<string> RotateRefreshTokenAsync(string refreshToken);
    }
}
