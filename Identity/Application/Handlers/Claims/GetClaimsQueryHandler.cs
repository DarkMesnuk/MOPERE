using Application.Dtos.Authentication;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Domain.Schemas.Claims;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Claims;

public class GetClaimsQueryHandler(
    ILogger<GetClaimsQueryHandler> logger, 
    IClaimsRepository claimsRepository, 
    IMapper mapper
) : BaseHandler<GetClaimsQueryHandler, GetClaimsQueryRequest, GetClaimsQueryResponse>(logger)
{
    public override async Task<GetClaimsQueryResponse> Handle(GetClaimsQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetClaimsQueryResponse(mapper);

        var claims = await claimsRepository.GetAsync(request, cancellationToken);

        return response.SetData(claims);
    }
}

public class GetClaimsQueryRequest : BaseAuthPaginatedHandlerRequest<GetClaimsQueryResponse>, IGetClaimsSchema
{
    public string? RoleId { get; init; }
}

public class GetClaimsQueryResponse(
    IMapper mapper
) : BasePaginatedQueryResponse<GetClaimsQueryResponse, ClaimDto, ClaimModel>(mapper);