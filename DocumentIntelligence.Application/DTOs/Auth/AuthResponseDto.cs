namespace DocumentIntelligence.Application.DTOs.Users
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
