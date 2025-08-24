using Base.API.Requests;
using Domain.Schemas.Users;

namespace Identity.API.Requests.Users.Admin;

public class UpdateUserByIdRequest : BaseAuthRequest, IUpdateUserSchema
{
    public string? Email { get; init; }
    
    public string? UserName { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    
    public bool? DeleteAvatar { get; init; }
}