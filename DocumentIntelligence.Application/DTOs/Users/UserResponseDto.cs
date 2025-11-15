namespace DocumentIntelligence.Application.DTOs.Users
{
    public class UserResponseDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
