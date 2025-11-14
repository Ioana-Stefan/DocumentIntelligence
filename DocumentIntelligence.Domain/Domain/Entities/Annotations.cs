using System;

namespace DocumentIntelligence.Domain.Entities
{
    public class Annotation
    {
        public Guid Id { get; set; }
        public Guid DocumentPageId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Label { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public DocumentPage DocumentPage { get; set; } = null!;
    }
}
