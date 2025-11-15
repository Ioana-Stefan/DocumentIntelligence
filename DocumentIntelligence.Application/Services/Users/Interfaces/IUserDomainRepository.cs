using DocumentIntelligence.Domain.Entities;

public interface IUserDomainRepository
{
    Task AddAsync(User user);
}