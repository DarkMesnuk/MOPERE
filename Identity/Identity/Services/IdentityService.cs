using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Base.Domain.Exceptions;
using Base.Domain.Helpers;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Contract.Consts;
using Domain.Exceptions;
using Domain.Models.Authentication;
using Domain.Schemas.Users;
using Identity.Configs;
using Identity.Extensions;
using Identity.Interfaces;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CodeExpiredException = Base.Domain.Exceptions.CodeExpiredException;

namespace Identity.Services;

public class IdentityService(
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager,
    RoleManager<RoleEntity> roleManager,
    IOptions<JwtConfigurations> jwtConfigurationsOption
) : IIdentityService
{
	private readonly JwtConfigurations _jwtConfigurations = jwtConfigurationsOption.Value;
	
	public bool Exists(string userId)
	{
		var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

		return user is not null;
	}

	public async Task<bool> ExistsByEmailAsync(string email)
	{
		var normalizedEmail = email.ToUpperInvariant();
		var user = await userManager.FindByEmailAsync(normalizedEmail);
			
		return user is not null;
	}

	private async Task<bool> ExistsByUserNameAsync(string username)
	{
		var normalizedUsername = username.ToUpperInvariant();
		var user = await userManager.FindByNameAsync(normalizedUsername);
			
		return user is not null;
	}

	public async Task<UserEntity?> GetByIdOrDefaultAsync(string userId)
	{
		var user = await userManager.FindByIdAsync(userId);

		if (user?.IsDeleted == true)
			user = null;
		
		return user;
	}

	public async Task<UserEntity> GetByIdAsync(string userId)
	{
		var user = await GetByIdOrDefaultAsync(userId);

		if (user.IsNullOrDefault())
			throw new NotFoundException(nameof(user), userId);
		
		return user!;
	}

	public async Task<UserEntity> GetByEmailAsync(string email)
	{
		var user = await userManager.FindByEmailAsync(email);

		if (user.IsNullOrDefault())
			throw new NotFoundException(nameof(user), email);
		
		return user!;
	}

	public async Task<UserEntity?> GetByEmailOrDefaultAsync(string email)
	{
		var user = await userManager.FindByEmailAsync(email);

		if (user?.IsDeleted == true)
			user = null;
		
		return user;
	}

	public async Task<UserEntity?> GetByEmailOrDefaultIfEvenDeletedAsync(string email)
	{
		var user = await userManager.FindByEmailAsync(email);
		
		return user;
	}

	public async Task<List<UserEntity>> GetByIdsAsync(IEnumerable<string> userIds)
	{
		var users = new List<UserEntity>();

		foreach (var userId in userIds)
		{
			var user = await GetByIdOrDefaultAsync(userId);

			if (user != null && !user.IsDeleted)
			{
				users.Add(user);
			}
		}

		return users;
	}

	public async Task ThrowIfUserNotPossibleToAuthAsync(string? userId)
	{
		if (!string.IsNullOrEmpty(userId))
		{
			var user = await GetByIdAsync(userId);

			if (user.IsNullOrDefault())
				throw new NotFoundException("UserModel", userId);

			if (user.IsDeleted)
				throw new AccountDeletedException();
			
			if (user.LockoutEnd > DateTimeOffset.Now)
				throw new BlockedException(user.LockoutEnd);
			
			if (user.IsBlocked)
				throw new BlockedException();
		}
		else
		{
			throw new UnauthorizedException();   
		}
	}

	public async Task<IdentityResult> CreateAndAssignRoleAsync(string email, string password, string roleName)
	{
		var (result, user) = await CreateAsync(email, password);
		
		if (result.Succeeded)
		{
			await AssignRoleAsync(user, roleName);
		}
		
		return result;
	}

	public Task<IdentityResult> CreateUserAsync(string email, string password)
	{
		return CreateAndAssignRoleAsync(email, password, RoleConsts.User); 
	}

	private async Task<(IdentityResult, UserEntity)> CreateAsync(string email, string password)
	{
		var now = DateTime.UtcNow;
		
		var cutEmail = email.Substring(0, email.IndexOf('@'));

		var userName = cutEmail;
		var userNameExist = await ExistsByUserNameAsync(userName);
		var index = 1;

		while (userNameExist)
		{
			userName = $"{cutEmail}{index}";
			userNameExist = await ExistsByUserNameAsync(userName);
			index++;
		}

		var user = new UserEntity
		{
			Email = email,
			NormalizedEmail = email.ToUpper(),
			UserName = userName,
			NormalizedUserName = userName.ToUpper(),
			ModifiedAt = now,
			CreatedAt = now
		};
        
		SetRandomVerificationCode(user);

		return (await userManager.CreateAsync(user, password), user);
	}

	public async Task<IdentityResult> AssignRoleAsync(UserEntity user, string roleName)
	{
		var role = await roleManager.FindByNameAsync(roleName);

		if (role.IsNullOrDefault())
		{
			role = new RoleEntity
			{
				Name = roleName,
				NormalizedName = roleName.ToUpper(),
				Id = Guid.NewGuid().ToString()
			};
			
			await roleManager.CreateAsync(role);
		}

		return await userManager.AddToRoleAsync(user, roleName);
	}

	public Task<SignInResult> CheckPasswordAsync(UserEntity user, string password)
	{
		return signInManager.CheckPasswordSignInAsync(user, password, false);
	}

	public async Task<IdentityResult> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword)
	{
		var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		
		await UpdatePasswordAtAsync(user, result);
		
		return result;
	}

	public async Task<IdentityResult> ResetPasswordAsync(UserEntity user, string newPassword)
	{
		var result = await userManager.ResetPasswordAsync(user, await userManager.GeneratePasswordResetTokenAsync(user), newPassword);
		
		await UpdatePasswordAtAsync(user, result);
		
		return result;
	}

	// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
	private Task UpdatePasswordAtAsync(UserEntity user, IdentityResult result)
	{
		if (!result.Succeeded)
		{
			throw new SomethingWentWrongException("Failed to change password");
		}
		
		return UpdateAsync(user);
	}

	public async Task<IdentityResult> ConfirmEmailAsync(UserEntity user)
	{
		return await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
	}

	public async Task<TokenModel> RefreshTokensAsync(string oldToken, string refreshToken)
	{
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtConfigurations.Issuer,
            ValidAudience = _jwtConfigurations.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.Key))
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(oldToken, tokenValidationParameters, out var validatedToken);

            if (validatedToken != null && claimsPrincipal != null)
            {
                tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.RefreshKey))
                };
                
                var claimsPrincipalRefresh = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedRefreshToken);
                
                if (validatedRefreshToken != null && claimsPrincipalRefresh != null)
                {
                    var isSameOwner = claimsPrincipal.Identity?.Name == claimsPrincipalRefresh.Identity?.Name;
                    
                    if (isSameOwner)
                    {
                        var user = await GetByIdAsync(claimsPrincipal.GetActiveUserId()!);

                        return await GetTokenAsync(user);
                    }
                }
            }
        }
        catch (Exception)
        {
	        // ignored
        }
        
        throw new UnauthorizedException();
	}

	public async Task<TokenModel> GetTokenAsync(string userId)
	{
		var user = await GetByIdAsync(userId);

		return await GetTokenAsync(user);
	}

	public async Task<TokenModel> GetTokenAsync(UserEntity user)
	{
		var claims = new List<Claim>
		{
			new (ClaimsIdentity.DefaultNameClaimType, user.UserName ?? string.Empty),
			new ("Id", user.Id),
			new ("AspNet.Identity.SecurityStamp", user.SecurityStamp ?? string.Empty),
			new ("auth_time", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
		};

		var roleNames = await userManager.GetRolesAsync(user);
		
		claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));
        
		var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		var now = DateTime.UtcNow;
		
		var accessTokenLifeTime = now.Add(TimeSpan.FromSeconds(_jwtConfigurations.LifeTimeSeconds));
		
		var accessJwt = new JwtSecurityToken(
			issuer: _jwtConfigurations.Issuer,
			audience: _jwtConfigurations.Audience,
			notBefore: now,
			claims: identity.Claims,
			expires: accessTokenLifeTime,
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfigurations.Key)), SecurityAlgorithms.HmacSha256));

		var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessJwt);
		
		var refreshTokenLifeTime = now.Add(TimeSpan.FromSeconds(_jwtConfigurations.RefreshLifeTimeSeconds));

		var refreshJwt = new JwtSecurityToken(
			notBefore: now,
			claims: identity.Claims,
			expires: refreshTokenLifeTime,
			signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfigurations.RefreshKey)), SecurityAlgorithms.HmacSha256));

		var refreshEncodedJwt = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

		return new TokenModel
		{
			AccessToken = encodedAccessJwt,
			AccessTokenLifeTime = accessTokenLifeTime,
			RefreshToken = refreshEncodedJwt,
			RefreshTokenLifeTime = refreshTokenLifeTime
		};
	}

	public async Task<bool> IsValidCodeAsync(UserEntity user, string code)
	{
		if (user.VerificationCountTries == 3)
		{
			throw new TooManyTriesException();
		}

		if (user.VerificationCodeLifeTime < DateTime.UtcNow)
		{
			throw new CodeExpiredException();
		}
		
		var isValid = user.VerificationCode == code;

		if (!isValid)
		{
			user.VerificationCountTries++;
			await UpdateAsync(user);
		}

		return isValid;
	}
	
	public void SetRandomVerificationCode(UserEntity user)
	{
		var code = StringGenerator.RandomStringNumber(6);
		
		user.VerificationCode = code;
		user.VerificationCodeLifeTime = DateTime.UtcNow.AddMinutes(10);
		user.VerificationCountTries = 0;
	}

	public Task<bool> SetRandomVerificationCodeAndSaveAsync(UserEntity user)
	{
		SetRandomVerificationCode(user);
		return UpdateAsync(user);
	}
	

	public async Task<bool> UpdateAsync(UserEntity user)
	{
		var identityResult = await userManager.UpdateAsync(user);

		if (!identityResult.Succeeded)
		{
			throw new UpdateFailedException();
		}
		
		return identityResult.Succeeded;
	}

	public async Task<bool> UpdateAsync(IEnumerable<UserEntity> users)
	{
		var isSuccess = true;
		
		foreach (var user in users)
		{
			var isUpdate = await UpdateAsync(user);

			if (!isUpdate && isSuccess)
			{
				isSuccess = false;
			}
		}

		return isSuccess;
	}

	public async Task<bool> UpdateBySchemaAsync(UserEntity user, IUpdateUserSchema schema)
	{
		if (schema.UserName != null)
		{
			var existsUserName = await ExistsByUserNameAsync(schema.UserName);

			if (!existsUserName)
			{
				user.UserName = schema.UserName ?? user.UserName;
				user.NormalizedUserName = schema.UserName?.Trim().ToUpperInvariant() ?? user.NormalizedUserName;		
			}
			else
			{
				throw new AlreadyExistsException("UserName already exists");
			}
		}

		user.FirstName = schema.FirstName ?? user.FirstName;
		user.LastName = schema.LastName ?? user.LastName;

		if (schema.DeleteAvatar == true)
		{
			user.AvatarUrl = null;
		}
        
		return await UpdateAsync(user);
	}

	public async Task<bool> DeleteAsync(UserEntity user)
	{
		var identityResult = await userManager.DeleteAsync(user);

		if (!identityResult.Succeeded)
		{
			throw new DeleteFailedException();
		}
		
		return identityResult.Succeeded;
	}

	public Task<bool> TemporaryDeleteAsync(UserEntity user)
	{

		return UpdateAsync(user);
	}

	public Task<bool> PermanentlyDeleteAsync(UserEntity user)
	{
		user.UserName = $"UN-{user.Id}";
		user.NormalizedUserName = $"UN-{user.Id}";
		user.Email = $"UN-{user.Id}@UN.UN";
		user.NormalizedEmail = $"UN-{user.Id}@UN.UN";
		user.PasswordHash = default;
		user.IsDeleted = true;
		user.DeletedAt = user.DeletedAt ?? DateTime.UtcNow;

		return UpdateAsync(user);
	}
}