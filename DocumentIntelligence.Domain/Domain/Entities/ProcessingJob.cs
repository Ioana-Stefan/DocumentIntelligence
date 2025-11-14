using System;

namespace DocumentIntelligence.Domain.Entities
{
    public enum JobType
    {
        OCR,
        Translation,
        Export
    }

    public enum JobStatus
    {
        Pending,
        Running,
        Completed,
        Failed
    }

    public class ProcessingJob
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public JobType JobType { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Document Document { get; set; } = null!;
    }
}
