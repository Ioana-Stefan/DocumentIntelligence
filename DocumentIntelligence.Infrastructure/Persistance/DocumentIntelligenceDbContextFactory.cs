using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DocumentIntelligence.Infrastructure.Persistence; // <-- make sure this is correct

namespace DocumentIntelligence.Infrastructure.Persistence
{
    public class DocumentIntelligenceDbContextFactory : IDesignTimeDbContextFactory<DocumentIntelligenceDbContext>
    {
        public DocumentIntelligenceDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING")
                                   ?? "Host=localhost;Database=DocumentIntelligence;Username=postgres;Password=password";

            var optionsBuilder = new DbContextOptionsBuilder<DocumentIntelligenceDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DocumentIntelligenceDbContext(optionsBuilder.Options);
        }
    }
}
