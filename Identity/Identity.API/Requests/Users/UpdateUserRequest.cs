using Base.API.Requests;
using Domain.Schemas.Users;

namespace Identity.API.Requests.Users;

public class UpdateUserRequest : BaseAuthRequest, IUpdateUserSchema
{
    public string? UserName { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public bool? DeleteAvatar { get; init; }
}