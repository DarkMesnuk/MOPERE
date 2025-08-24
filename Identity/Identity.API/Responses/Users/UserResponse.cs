using Application.Dtos.Users;
using Base.API.Responses;

namespace Identity.API.Responses.Users;

public class UserResponse(UserDto dto) : BaseResponse<UserDto>(dto);