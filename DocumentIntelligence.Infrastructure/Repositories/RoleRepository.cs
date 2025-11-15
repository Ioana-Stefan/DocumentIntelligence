using DocumentIntelligence.Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<bool> RoleExistsAsync(string role) => _roleManager.RoleExistsAsync(role);

        public Task<IdentityResult> CreateRoleAsync(string role) => _roleManager.CreateAsync(new IdentityRole(role));
    }
}
