using Base.API.Requests;

namespace Identity.API.Requests.Claims;

public class CreateClaimRequest : BaseAuthRequest
{
    public required string Type { get; init; }
    public required string Value { get; init; }
}