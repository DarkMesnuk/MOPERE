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

namespace Application.Handlers.Users.Admin;

public class GetUsersAdminQueryHandler(
    ILogger<GetUsersAdminQueryHandler> logger,
    IMapper mapper,
    IUsersRepository usersRepository
) : BaseHandler<GetUsersAdminQueryHandler, GetUsersAdminQueryRequest, GetUsersAdminQueryResponse>(logger)
{
    public override async Task<GetUsersAdminQueryResponse> Handle(GetUsersAdminQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetUsersAdminQueryResponse(mapper);

        var users = await usersRepository.GetAsync(request, cancellationToken);

        return response.SetData(users);
    }
}

public class GetUsersAdminQueryRequest : BaseAuthPaginatedHandlerRequest<GetUsersAdminQueryResponse>, IGetUsersAdminSchema
{
    public string? Search { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public string? Sort { get; init; }
    public SortDirection? SortDirection { get; init; }
}

public class GetUsersAdminQueryResponse(
    IMapper mapper
) : BasePaginatedQueryResponse<GetUsersAdminQueryResponse, UserDto, UserModel>(mapper);