using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IJwtTokenService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { error = "Invalid credentials" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized(new { error = "Invalid credentials" });

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, roles);

            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);
            var response = new UserResponseDto{
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                Roles = roles.ToArray(),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var user = await _refreshTokenService.ValidateRefreshTokenAsync(dto.Token);
            if (user == null)
                return Unauthorized(new { error = "Invalid or expired refresh token" });

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtService.GenerateToken(user, roles);
            var newRefreshToken = await _refreshTokenService.RotateRefreshTokenAsync(dto.Token);

            var response = new UserResponseDto{
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName,
                Roles = roles.ToArray(),
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto dto)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(dto.Token);
            return NoContent();
        }
    }
}
