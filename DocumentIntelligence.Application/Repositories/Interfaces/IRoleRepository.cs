using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Repositories
{
    public interface IRoleRepository
    {
        // Basic role operations
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);

        // Retrieve roles
        Task<IdentityRole?> GetByNameAsync(string roleName);
        Task<IdentityRole?> GetByIdAsync(string roleId);
        Task<IList<IdentityRole>> GetAllRolesAsync();
    }
}
