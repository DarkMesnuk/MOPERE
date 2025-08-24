using Application.Dtos.Authentication;
using Base.API.Responses;
using Base.Domain.Schemas.Interfaces.Responses;

namespace Identity.API.Responses.Roles;

public class RolesResponse(IPaginatedResponse<RoleDto> dto) : BaseResponse<IPaginatedResponse<RoleDto>>(dto);