using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Users.Interfaces;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRegistrationService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto, string role = "User")
        {
            var user = new Microsoft.AspNetCore.Identity.IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userRepository.CreateUserAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"User creation failed: {errors}");
            }

            if (!await _roleRepository.RoleExistsAsync(role))
            {
                await _roleRepository.CreateRoleAsync(role);
            }

            await _userRepository.AddToRoleAsync(user, role);

            var roles = await _userRepository.GetRolesAsync(user);
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToArray()
            };
        }
    }
}
