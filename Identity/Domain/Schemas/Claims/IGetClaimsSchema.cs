using Base.Domain.Schemas.Interfaces.Requests;

namespace Domain.Schemas.Claims;

public interface IGetClaimsSchema : IPaginatedRequest
{
    string? RoleId { get; init; }
}