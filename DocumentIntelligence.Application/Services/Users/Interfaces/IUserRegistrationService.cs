using DocumentIntelligence.Application.DTOs.Users;

namespace DocumentIntelligence.Application.Services.Users.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto, string role = "User");
    }
}
