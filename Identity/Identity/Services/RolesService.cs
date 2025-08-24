using Domain.Interfaces.Repositories.Authentication;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Authentication;
using Domain.Models.Users;
using Identity.Interfaces;

namespace Identity.Services;

public class RolesService(
    IClaimsRepository claimsRepository,
    IUserClaimsRepository userClaimsRepository,
    IUserRolesRepository userRolesRepository,
    IUsersRepository usersRepository,
    IRolesRepository rolesRepository
) : IRolesService
{
    public async Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default)
    {
        await usersRepository.ExistsOrThrowAsync(userId);
        
        await userRolesRepository.DeleteByUserAsync(userId);

        await userClaimsRepository.DeleteByUserAsync(userId);
        
        await rolesRepository.ExistsOrThrowAsync(roleId);

        var user = new UserModel { Id = userId };
        
        await userRolesRepository.CreateAsync(new UserRoleModel
        {
            User = user,
            Role = new RoleModel {Id = roleId}
        });

        var claims = await claimsRepository.GetByRoleIdAsync(roleId, cancellationToken);

        if (claims.Count != 0)
        {
            await userClaimsRepository.CreateAsync(claims.Select(x => new UserClaimModel
            {
                User = user,
                Claim = x
            }));
        }
    }
}