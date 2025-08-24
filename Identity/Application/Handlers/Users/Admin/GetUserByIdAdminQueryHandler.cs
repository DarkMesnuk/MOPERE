using Application.Dtos.Users;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users.Admin;

public class GetUserByIdAdminQueryHandler(
    ILogger<GetUserByIdAdminQueryHandler> logger,
    IMapper mapper,
    IUsersRepository usersRepository
) : BaseHandler<GetUserByIdAdminQueryHandler, GetUserByIdAdminQueryRequest, GetUserByIdAdminQueryResponse>(logger)
{
    public override async Task<GetUserByIdAdminQueryResponse> Handle(GetUserByIdAdminQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetUserByIdAdminQueryResponse(mapper);
        
        var user = await usersRepository.GetWithIncludesByIdAsync(request.UserId, cancellationToken);

        return response.SetData(user);
    }
}

public class GetUserByIdAdminQueryRequest : BaseAuthHandlerRequest<GetUserByIdAdminQueryResponse>
{
    public string UserId { get; set; } // FromRoute
}

public class GetUserByIdAdminQueryResponse(
    IMapper mapper
) : BaseQueryResponse<GetUserByIdAdminQueryResponse, UserAdminDto, UserModel>(mapper);