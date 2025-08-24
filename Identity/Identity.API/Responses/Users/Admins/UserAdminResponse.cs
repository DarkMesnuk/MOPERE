using Application.Dtos.Users;
using Base.API.Responses;

namespace Identity.API.Responses.Users.Admins;

public class UserAdminResponse(UserAdminDto dto) : BaseResponse<UserAdminDto>(dto);