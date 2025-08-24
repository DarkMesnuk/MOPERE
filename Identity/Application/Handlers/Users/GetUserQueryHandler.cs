using Application.Dtos.Users;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users;

public class GetUserQueryHandler(
    ILogger<GetUserQueryHandler> logger,
    IMapper mapper,
    IUsersRepository usersRepository
) : BaseHandler<GetUserQueryHandler, GetUserQueryRequest, GetUserQueryResponse>(logger)
{
    public override async Task<GetUserQueryResponse> Handle(GetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetUserQueryResponse(mapper);

        var user = await usersRepository.GetByIdAsync(request.AuthUserId, cancellationToken);

        return response.SetData(user);
    }
}

public class GetUserQueryRequest : BaseAuthHandlerRequest<GetUserQueryResponse>;

public class GetUserQueryResponse(
    IMapper mapper
) : BaseQueryResponse<GetUserQueryResponse, UserDto, UserModel>(mapper);