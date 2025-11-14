using System;
using System.Collections.Generic;

namespace DocumentIntelligence.Domain.Entities
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
