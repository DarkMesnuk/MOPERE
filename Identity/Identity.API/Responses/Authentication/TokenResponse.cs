using Application.Dtos.Authentication;
using Base.API.Responses;

namespace Identity.API.Responses.Authentication;

public class TokenResponse(TokenDto dto) : BaseResponse<TokenDto>(dto);