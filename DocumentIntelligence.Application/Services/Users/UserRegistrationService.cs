using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Users.Interfaces;
using DocumentIntelligence.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserDomainRepository _userDomainRepository;

        public UserRegistrationService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserDomainRepository userDomainRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userDomainRepository = userDomainRepository;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto, string role = "User")
        {
            var existingIdentityUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingIdentityUser != null)
                throw new InvalidOperationException("Email already registered.");

            var domainUserId = Guid.NewGuid();

            var identityUser = new IdentityUser
            {
                Id = domainUserId.ToString(),
                UserName = dto.Email,
                Email = dto.Email
            };

            var createResult = await _userRepository.CreateUserAsync(identityUser, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var domainUser = new User
            {
                Id = domainUserId,
                Email = dto.Email,
                Name = dto.Name
            };


            await _userDomainRepository.AddAsync(domainUser);

            if (!await _roleRepository.RoleExistsAsync(role))
            {
                var roleResult = await _roleRepository.CreateRoleAsync(role);
                if (!roleResult.Succeeded)
                    throw new InvalidOperationException($"Failed to create role {role}");
            }

            var addToRoleResult = await _userRepository.AddToRoleAsync(identityUser, role);
            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign role: {errors}");
            }

            var roles = await _userRepository.GetRolesAsync(identityUser);

            var userDto = new UserResponseDto
            {
                Id = domainUser.Id.ToString(),
                Email = domainUser.Email,
                Name = domainUser.Name,
                Roles = roles.ToArray(),
                AccessToken = null,
                RefreshToken = null
            };

            return userDto;
        }

    }
}
