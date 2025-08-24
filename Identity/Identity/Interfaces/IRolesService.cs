namespace Identity.Interfaces;

public interface IRolesService
{
    Task AssignRoleToUserAsync(string userId, string roleId, CancellationToken cancellationToken = default);
}