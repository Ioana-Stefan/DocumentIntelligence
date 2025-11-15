using DocumentIntelligence.Application.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocumentIntelligence.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // ------------------------
        // Basic role operations
        // ------------------------
        public async Task<bool> RoleExistsAsync(string roleName)
            => await _roleManager.RoleExistsAsync(roleName);

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' already exists." });

            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' does not exist." });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id);
            if (existingRole == null)
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{role.Name}' does not exist." });

            existingRole.Name = role.Name;
            existingRole.NormalizedName = role.Name.ToUpperInvariant();

            return await _roleManager.UpdateAsync(existingRole);
        }

        // ------------------------
        // Retrieve roles
        // ------------------------
        public async Task<IdentityRole?> GetByNameAsync(string roleName)
            => await _roleManager.FindByNameAsync(roleName);

        public async Task<IdentityRole?> GetByIdAsync(string roleId)
            => await _roleManager.FindByIdAsync(roleId);

        public async Task<IList<IdentityRole>> GetAllRolesAsync()
            => await _roleManager.Roles.ToListAsync();
    }
}
