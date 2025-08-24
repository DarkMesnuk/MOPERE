using Application.Dtos.Authentication;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Repositories.Authentication;
using Domain.Models.Authentication;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Roles;

public class GetRolesQueryHandler(
    ILogger<GetRolesQueryHandler> logger, 
    IRolesRepository rolesRepository, 
    IMapper mapper
) : BaseHandler<GetRolesQueryHandler, GetRolesCommandRequest, GetRolesQueryResponse>(logger)
{
    public override async Task<GetRolesQueryResponse> Handle(GetRolesCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new GetRolesQueryResponse(mapper);
        
        var roles = await rolesRepository.GetAsync(request, cancellationToken);

        return response.SetData(roles);
    }
}

public class GetRolesCommandRequest : BaseAuthPaginatedHandlerRequest<GetRolesQueryResponse>;

public class GetRolesQueryResponse(
    IMapper mapper
) : BasePaginatedQueryResponse<GetRolesQueryResponse, RoleDto, RoleModel>(mapper);