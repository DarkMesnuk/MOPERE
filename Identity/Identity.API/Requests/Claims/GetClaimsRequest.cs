using Base.API.Requests;
using Domain.Schemas.Claims;

namespace Identity.API.Requests.Claims;

public class GetClaimsRequest : BaseAuthPaginatedRequest, IGetClaimsSchema
{
    public string? RoleId { get; init; }
}