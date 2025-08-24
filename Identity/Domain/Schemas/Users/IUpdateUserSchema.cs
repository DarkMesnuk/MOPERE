namespace Domain.Schemas.Users;

public interface IUpdateUserSchema
{
    public string? UserName { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    
    public bool? DeleteAvatar { get; init; }
}