namespace DocumentIntelligence.Infrastructure.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}
