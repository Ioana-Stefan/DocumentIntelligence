using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityUser?> GetByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<IList<string>> GetRolesAsync(IdentityUser user);
        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);
    }
}
