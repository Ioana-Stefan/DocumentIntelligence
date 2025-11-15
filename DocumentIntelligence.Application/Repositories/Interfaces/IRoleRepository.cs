using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> RoleExistsAsync(string role);
        Task<IdentityResult> CreateRoleAsync(string role);
    }
}
