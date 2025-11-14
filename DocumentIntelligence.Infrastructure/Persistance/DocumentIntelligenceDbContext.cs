using DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentIntelligence.Infrastructure.Persistence
{
    public class DocumentIntelligenceDbContext : DbContext
    {
        public DocumentIntelligenceDbContext(DbContextOptions<DocumentIntelligenceDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<DocumentPage> DocumentPages { get; set; } = null!;
        public DbSet<Annotation> Annotations { get; set; } = null!;
        public DbSet<ProcessingJob> ProcessingJobs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).HasConversion<string>().IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
            });

            // Documents
            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("documents");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FileName).IsRequired().HasMaxLength(255);
                entity.Property(d => d.OriginalFilePath).IsRequired();
                entity.Property(d => d.Status).HasConversion<string>().IsRequired();
                entity.Property(d => d.LanguageDetected);
                entity.Property(d => d.CreatedAt).IsRequired();

                entity.HasOne(d => d.User)
                      .WithMany(u => u.Documents)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // DocumentPages
            modelBuilder.Entity<DocumentPage>(entity =>
            {
                entity.ToTable("document_pages");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.PageNumber).IsRequired();
                entity.Property(p => p.ImagePath).IsRequired();
                entity.Property(p => p.TextExtracted);
                entity.Property(p => p.CreatedAt).IsRequired();

                entity.HasOne(p => p.Document)
                      .WithMany(d => d.Pages)
                      .HasForeignKey(p => p.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Annotations
            modelBuilder.Entity<Annotation>(entity =>
            {
                entity.ToTable("annotations");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.X).IsRequired();
                entity.Property(a => a.Y).IsRequired();
                entity.Property(a => a.Width).IsRequired();
                entity.Property(a => a.Height).IsRequired();
                entity.Property(a => a.Label).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Comment);
                entity.Property(a => a.CreatedAt).IsRequired();

                entity.HasOne(a => a.DocumentPage)
                      .WithMany(p => p.Annotations)
                      .HasForeignKey(a => a.DocumentPageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProcessingJobs
            modelBuilder.Entity<ProcessingJob>(entity =>
            {
                entity.ToTable("processing_jobs");
                entity.HasKey(j => j.Id);
                entity.Property(j => j.JobType).HasConversion<string>().IsRequired();
                entity.Property(j => j.Status).HasConversion<string>().IsRequired();
                entity.Property(j => j.CreatedAt).IsRequired();

                entity.HasOne(j => j.Document)
                      .WithMany(d => d.ProcessingJobs)
                      .HasForeignKey(j => j.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
