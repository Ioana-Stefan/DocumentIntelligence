using System;
using System.Collections.Generic;

namespace DocumentIntelligence.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name {get; set;} = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
