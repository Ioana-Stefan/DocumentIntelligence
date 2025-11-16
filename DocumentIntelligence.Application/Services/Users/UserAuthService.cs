using DocumentIntelligence.Application.DTOs.Users;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Auth.Interfaces;

public class UserAuthService : IUserAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public UserAuthService(
        IUserRepository userRepository,
        IJwtTokenService tokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<UserResponseDto> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var valid = await _userRepository.CheckPasswordAsync(user, password);
        if (!valid)
            throw new UnauthorizedAccessException("Invalid credentials");

        var roles = await _userRepository.GetRolesAsync(user);

        var accessToken = _tokenService.GenerateToken(user, roles);
        var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.UserName,
            Roles = roles.ToArray(),
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<UserResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var user = await _refreshTokenService.ValidateRefreshTokenAsync(refreshToken);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var roles = await _userRepository.GetRolesAsync(user);

        var newAccessToken = _tokenService.GenerateToken(user, roles);
        var newRefreshToken = await _refreshTokenService.RotateRefreshTokenAsync(refreshToken);

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.UserName,
            Roles = roles.ToArray(),
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        return _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
    }

    public bool IsTokenValid(string token)
    {
        var result = _tokenService.ValidateToken(token);
        return result.IsValid;
    }

}
