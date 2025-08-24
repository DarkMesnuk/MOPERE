using Base.API.Requests;

namespace Identity.API.Requests.Roles;

public class UpdateRoleRequest : BaseAuthRequest
{
    public string? Name { get; init; }
}