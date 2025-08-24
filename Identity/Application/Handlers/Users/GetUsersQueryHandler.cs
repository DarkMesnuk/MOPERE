using Application.Dtos.Users;
using AutoMapper;
using Base.Application;
using Base.Application.Requests;
using Base.Application.Responses;
using Base.Domain.Enums;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using Domain.Schemas.Users;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Users;

public class GetUsersQueryHandler(
    ILogger<GetUsersQueryHandler> logger,
    IMapper mapper,
    IUsersRepository usersRepository
) : BaseHandler<GetUsersQueryHandler, GetUsersQueryRequest, GetUsersQueryResponse>(logger)
{
    public override async Task<GetUsersQueryResponse> Handle(GetUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetUsersQueryResponse(mapper);

        var users = await usersRepository.GetAsync(request, cancellationToken);

        return response.SetData(users);
    }
}

public class GetUsersQueryRequest : BaseAuthPaginatedHandlerRequest<GetUsersQueryResponse>, IGetUsersSchema
{
    public string? Search { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public string? Sort { get; init; }
    public SortDirection? SortDirection { get; init; }
}

public class GetUsersQueryResponse(
    IMapper mapper
) : BasePaginatedQueryResponse<GetUsersQueryResponse, UserDto, UserModel>(mapper);