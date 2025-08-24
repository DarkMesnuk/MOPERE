using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class UpdateClaimRequest : BaseAuthRequest
{
    public string? Type { get; init; }
    public string? Value { get; init; }
}