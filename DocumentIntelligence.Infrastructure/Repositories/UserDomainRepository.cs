using DocumentIntelligence.Domain.Entities;
using DocumentIntelligence.Infrastructure.Persistence;

public class UserDomainRepository : IUserDomainRepository
{
    private readonly DocumentIntelligenceDbContext _db;

    public UserDomainRepository(DocumentIntelligenceDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}
