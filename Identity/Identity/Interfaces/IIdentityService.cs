using Domain.Models.Authentication;
using Domain.Schemas.Users;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Interfaces;

public interface IIdentityService
{
    bool Exists(string userId);
    Task<bool> ExistsByEmailAsync(string email);
    Task<UserEntity?> GetByIdOrDefaultAsync(string userId);
    Task<UserEntity> GetByIdAsync(string userId);
    Task<UserEntity> GetByEmailAsync(string email);
    Task<UserEntity?> GetByEmailOrDefaultAsync(string email);
    Task<UserEntity?> GetByEmailOrDefaultIfEvenDeletedAsync(string email);
    Task<List<UserEntity>> GetByIdsAsync(IEnumerable<string> userIds);

    Task ThrowIfUserNotPossibleToAuthAsync(string? userId);

    Task<IdentityResult> CreateAndAssignRoleAsync(string email, string password, string roleName);
    Task<IdentityResult> CreateUserAsync(string email, string password);
    
    Task<IdentityResult> AssignRoleAsync(UserEntity user, string roleName);
    
    Task<SignInResult> CheckPasswordAsync(UserEntity user, string password);
    
    Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
    Task<IdentityResult> ResetPasswordAsync(UserEntity user, string newPassword);
    
    Task<IdentityResult> ConfirmEmailAsync(UserEntity user);
    
    Task<TokenModel> RefreshTokensAsync(string oldToken, string refreshToken);
    
    Task<TokenModel> GetTokenAsync(string userId);
    Task<TokenModel> GetTokenAsync(UserEntity user);
    
    Task<bool> IsValidCodeAsync(UserEntity user, string code);
    void SetRandomVerificationCode(UserEntity user);
    Task<bool> SetRandomVerificationCodeAndSaveAsync(UserEntity user);
    
    Task<bool> UpdateAsync(UserEntity user);
    Task<bool> UpdateAsync(IEnumerable<UserEntity> users);
    
    Task<bool> UpdateBySchemaAsync(UserEntity user, IUpdateUserSchema schema);
    
    Task<bool> DeleteAsync(UserEntity user);
    Task<bool> PermanentlyDeleteAsync(UserEntity user);
    Task<bool> TemporaryDeleteAsync(UserEntity user);
}