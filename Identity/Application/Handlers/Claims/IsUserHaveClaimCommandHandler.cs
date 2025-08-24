using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Repositories.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class IsUserHaveClaimCommandHandler(
    ILogger<IsUserHaveClaimCommandHandler> logger,
    IUserClaimsRepository userClaimsRepository
) : BaseHandler<IsUserHaveClaimCommandHandler, IsUserHaveClaimCommandRequest, IsUserHaveClaimCommandResponse>(logger)
{
    public override async Task<IsUserHaveClaimCommandResponse> Handle(IsUserHaveClaimCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new IsUserHaveClaimCommandResponse();
        
        var isUserHaveClaim = await userClaimsRepository.ExistsAsync(request.UserId, request.ClaimId, cancellationToken);

        return response.SetData(isUserHaveClaim);
    }
}

public class IsUserHaveClaimCommandRequest : BaseAuthHandlerRequest<IsUserHaveClaimCommandResponse>
{
    public required string UserId { get; init; }
    public required string ClaimId { get; init; }
}

public class IsUserHaveClaimCommandResponse : BaseQueryResponse<IsUserHaveClaimCommandResponse, bool>;