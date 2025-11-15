using DocumentIntelligence.Application.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Infrastructure.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityUser?> GetByEmailAsync(string email) => _userManager.FindByEmailAsync(email);

        public Task<IdentityResult> CreateUserAsync(IdentityUser user, string password) =>
            _userManager.CreateAsync(user, password);

        public Task<IList<string>> GetRolesAsync(IdentityUser user) => _userManager.GetRolesAsync(user);

        public Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role) =>
            _userManager.AddToRoleAsync(user, role);
    }
}
