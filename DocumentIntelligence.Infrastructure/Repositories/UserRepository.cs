using DocumentIntelligence.Application.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DocumentIntelligence.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // ------------------------
        // Basic user operations
        // ------------------------
        public async Task<IdentityUser?> GetByIdAsync(string id)
            => await _userManager.FindByIdAsync(id);

        public async Task<IdentityUser?> GetByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<IdentityUser?> GetByUsernameAsync(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
            => await _userManager.UpdateAsync(user);

        public async Task<IdentityResult> DeleteUserAsync(IdentityUser user)
            => await _userManager.DeleteAsync(user);

        // ------------------------
        // Password operations
        // ------------------------
        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword)
            => await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        public async Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword)
            => await _userManager.ResetPasswordAsync(user, token, newPassword);

        public async Task<string> GeneratePasswordResetTokenAsync(IdentityUser user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        // ------------------------
        // Role operations
        // ------------------------
        public async Task<IList<string>> GetRolesAsync(IdentityUser user)
            => await _userManager.GetRolesAsync(user);

        public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role)
            => await _userManager.AddToRoleAsync(user, role);

        public async Task<IdentityResult> RemoveFromRoleAsync(IdentityUser user, string role)
            => await _userManager.RemoveFromRoleAsync(user, role);

        public async Task<bool> IsInRoleAsync(IdentityUser user, string role)
            => await _userManager.IsInRoleAsync(user, role);

        // ------------------------
        // Claims operations
        // ------------------------
        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
            => await _userManager.GetClaimsAsync(user);

        public async Task<IdentityResult> AddClaimAsync(IdentityUser user, Claim claim)
            => await _userManager.AddClaimAsync(user, claim);

        public async Task<IdentityResult> RemoveClaimAsync(IdentityUser user, Claim claim)
            => await _userManager.RemoveClaimAsync(user, claim);

        // ------------------------
        // Lockout / access
        // ------------------------
        public async Task<IdentityResult> SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd)
            => await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

        public async Task<bool> IsLockedOutAsync(IdentityUser user)
            => await _userManager.IsLockedOutAsync(user);

        public async Task<IdentityResult> AccessFailedAsync(IdentityUser user)
            => await _userManager.AccessFailedAsync(user);

        public async Task<IdentityResult> ResetAccessFailedCountAsync(IdentityUser user)
            => await _userManager.ResetAccessFailedCountAsync(user);

        public async Task<int> GetAccessFailedCountAsync(IdentityUser user)
            => await _userManager.GetAccessFailedCountAsync(user);
    }
}
