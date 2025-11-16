using DocumentIntelligence.Application.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocumentIntelligence.Infrastructure.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IdentityUser user, IEnumerable<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (bool IsValid, string? UserId, string? Email, IEnumerable<string> Roles) ValidateToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParams, out _);

                var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var email  = principal.FindFirstValue(JwtRegisteredClaimNames.Email);
                var roles  = principal.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray();

                return (true, userId, email, roles);
            }
            catch
            {
                return (false, null, null, Array.Empty<string>());
            }
        }
    }
}
