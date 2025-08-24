using Base.API.Requests;
using Base.Domain.Enums;
using Domain.Schemas.Users;

namespace Identity.API.Requests.Users.Admin;

public class GetUsersAdminRequest : BaseAuthPaginatedRequest, IGetUsersAdminSchema
{
    public string? Search { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public string? Sort { get; init; }
    public SortDirection? SortDirection { get; init; }
}