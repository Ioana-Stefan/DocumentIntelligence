using DocumentIntelligence.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _authService;

        public AuthController(IUserAuthService authService)
        {
           _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
        try
            {
                var result = await _authService.LoginAsync(dto.Email, dto.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(dto.Token);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto dto)
        {
            await _authService.RevokeRefreshTokenAsync(dto.Token);
            return NoContent();
        }
    }
}
