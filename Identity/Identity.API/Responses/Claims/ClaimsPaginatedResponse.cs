using Application.Dtos.Authentication;
using Base.API.Responses;
using Base.Domain.Schemas.Interfaces.Responses;

namespace Identity.API.Responses.Claims;

public class ClaimsPaginatedResponse(IPaginatedResponse<ClaimDto> dto) : BaseResponse<IPaginatedResponse<ClaimDto>>(dto);