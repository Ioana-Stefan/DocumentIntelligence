using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Auth.Interfaces;
using DocumentIntelligence.Application.Services.Users.Interfaces;
using DocumentIntelligence.Domain.Entities;
using DocumentIntelligence.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly DocumentIntelligenceDbContext _db;

        public UserRegistrationService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService,
            DocumentIntelligenceDbContext db)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
            _db = db;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto, string role = "User")
        {
            // Check if email exists in Identity
            var existingIdentityUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingIdentityUser != null)
                throw new InvalidOperationException("Email already registered.");

            // Generate a GUID for your domain user
            var domainUserId = Guid.NewGuid();

            // Create IdentityUser, use GUID as string
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

            // Create domain user entry
            var domainUser = new User
            {
                Id = domainUserId,
                Email = dto.Email,
                Name = dto.Name
            };

            _db.Users.Add(domainUser);
            await _db.SaveChangesAsync();

            // Roles
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
