using DocumentIntelligence.Application.DTOs.Users;

public interface IUserAuthService
{
    Task<UserResponseDto> LoginAsync(string email, string password);
    Task<UserResponseDto> RefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    public bool IsTokenValid(string token);

}
