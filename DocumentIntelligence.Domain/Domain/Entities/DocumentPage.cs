using System;
using System.Collections.Generic;

namespace DocumentIntelligence.Domain.Entities
{
    public class DocumentPage
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int PageNumber { get; set; }
        public string ImagePath { get; set; } = null!;
        public string? TextExtracted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Document Document { get; set; } = null!;
        public ICollection<Annotation> Annotations { get; set; } = new List<Annotation>();
    }
}
