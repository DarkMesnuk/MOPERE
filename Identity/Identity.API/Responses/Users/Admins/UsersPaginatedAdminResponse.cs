using Application.Dtos.Users;
using Base.API.Responses;
using Base.Domain.Schemas.Interfaces.Responses;

namespace Identity.API.Responses.Users.Admins;

public class UsersPaginatedAdminResponse(IPaginatedResponse<UserAdminDto> dto) : BaseResponse<IPaginatedResponse<UserAdminDto>>(dto);