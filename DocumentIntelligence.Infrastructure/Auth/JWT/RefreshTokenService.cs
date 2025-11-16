using DocumentIntelligence.Application.Services.Auth.Interfaces;
using DocumentIntelligence.Infrastructure.Auth;
using DocumentIntelligence.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace DocumentIntelligence.Infrastructure.Services.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DocumentIntelligenceDbContext _db;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(
            UserManager<IdentityUser> userManager,
            DocumentIntelligenceDbContext db,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _db = db;
            _configuration = configuration;
        }

        public async Task<string> CreateRefreshTokenAsync(IdentityUser user)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"])
                )
            };

            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            return token;
        }

        public async Task<IdentityUser> ValidateRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
                return null;

            return await _userManager.FindByIdAsync(tokenEntity.UserId);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (tokenEntity == null)
                return;

            tokenEntity.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task<string> RotateRefreshTokenAsync(string refreshToken)
        {
            var oldToken = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (oldToken == null || !oldToken.IsActive)
                return null;

            oldToken.RevokedAt = DateTime.UtcNow;

            var newToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = oldToken.UserId,
                Token = newToken,
                CreatedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"])
                )
            });

            await _db.SaveChangesAsync();

            return newToken;
        }
    }
}
