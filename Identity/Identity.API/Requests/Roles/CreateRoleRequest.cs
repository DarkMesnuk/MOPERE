using Base.API.Requests;

namespace Identity.API.Requests.Roles;

public class CreateRoleRequest : BaseAuthRequest
{
    public required string Name { get; init; }
}