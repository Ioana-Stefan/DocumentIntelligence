using DocumentIntelligence.Application.DTOs.Users;

namespace DocumentIntelligence.Application.Services.Users.Interfaces
{
    public interface IUserAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginUserDto dto);
    }
}
