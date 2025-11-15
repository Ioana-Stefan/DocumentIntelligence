using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRegistrationService _registrationService;

        public UsersController(IUserRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                var user = await _registrationService.RegisterUserAsync(dto);
                return CreatedAtAction(nameof(Register), user); // 201 Created
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
