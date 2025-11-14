using System;
using System.Collections.Generic;

namespace DocumentIntelligence.Domain.Entities
{
    public enum DocumentStatus
    {
        Uploaded,
        Processing,
        Completed,
        Failed
    }

    public class Document
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; } = null!;
        public string OriginalFilePath { get; set; } = null!;
        public DocumentStatus Status { get; set; } = DocumentStatus.Uploaded;
        public string? LanguageDetected { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<DocumentPage> Pages { get; set; } = new List<DocumentPage>();
        public ICollection<ProcessingJob> ProcessingJobs { get; set; } = new List<ProcessingJob>();
    }
}
