using System.Security.Authentication;
using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Auth.Interfaces;
using DocumentIntelligence.Application.Services.Users.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new AuthenticationException("Invalid credentials");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new AuthenticationException("Invalid credentials");

            var roles = await _userRepository.GetRolesAsync(user);
            var token = _jwtTokenService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                Email = user.Email,
                Roles = roles.ToArray(),
                Token = token
            };
        }
    }
}


