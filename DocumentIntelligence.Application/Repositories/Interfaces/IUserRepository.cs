using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DocumentIntelligence.Application.Repositories
{
    public interface IUserRepository
    {
        // Basic user operations
        Task<IdentityUser?> GetByIdAsync(string id);
        Task<IdentityUser?> GetByEmailAsync(string email);
        Task<IdentityUser?> GetByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<IdentityResult> UpdateUserAsync(IdentityUser user);
        Task<IdentityResult> DeleteUserAsync(IdentityUser user);

        // Password operations
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(IdentityUser user, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);

        // Role operations
        Task<IList<string>> GetRolesAsync(IdentityUser user);
        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(IdentityUser user, string role);
        Task<bool> IsInRoleAsync(IdentityUser user, string role);

        // Claims operations
        Task<IList<Claim>> GetClaimsAsync(IdentityUser user);
        Task<IdentityResult> AddClaimAsync(IdentityUser user, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(IdentityUser user, Claim claim);

        // Lockout / access
        Task<IdentityResult> SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd);
        Task<bool> IsLockedOutAsync(IdentityUser user);
        Task<IdentityResult> AccessFailedAsync(IdentityUser user);
        Task<IdentityResult> ResetAccessFailedCountAsync(IdentityUser user);
        Task<int> GetAccessFailedCountAsync(IdentityUser user);
    }
}
