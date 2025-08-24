using Application.Dtos.Users;
using Base.API.Responses;
using Base.Domain.Schemas.Interfaces.Responses;

namespace Identity.API.Responses.Users;

public class UsersPaginatedResponse(IPaginatedResponse<UserDto> dto) : BaseResponse<IPaginatedResponse<UserDto>>(dto);