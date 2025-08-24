using Identity.API.Requests.Users.Admin;
using Identity.API.Responses.Users;
using Identity.API.Responses.Users.Admins;
using Application.Handlers.Users.Admin;
using AutoMapper;
using Base.API.Controllers;
using Contract.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers.Admin;

[Authorize(Roles = RoleConsts.SuperAdmin)]
[Route("api/v1/admin/user")]
public class UsersAdminController(
    ILogger<UsersAdminController> logger, 
    IMapper mapper, 
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// [Admin] Get users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UsersPaginatedResponse), 200)]
    public async Task<IActionResult> Get([FromQuery] GetUsersAdminRequest request)
    {
        var applicationRequest = Mapper.Map<GetUsersAdminQueryRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new UsersPaginatedResponse(applicationResponse);

        return Ok(response);
    }
    
    /// <summary>
    /// [Admin] Get user by id
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(UserAdminResponse), 200)]
    public async Task<IActionResult> GetById([FromQuery] GetUserByIdAdminRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<GetUserByIdAdminQueryRequest>(request);
        applicationRequest.UserId = id;

        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new UserAdminResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// [Admin] Create admin
    /// </summary>
    [HttpPost]
    [Route("admin")]
    public async Task<IActionResult> CreateAdmin([FromQuery] CreateAdminUserRequest request)
    {
        var applicationRequest = Mapper.Map<CreateAdminUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Update user by userId 
    /// </summary>
    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateUserByIdRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UpdateUserByIdCommandRequest>(request);
        applicationRequest.UserId = id;

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Delete user by userId 
    /// </summary>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] DeleteUserByIdRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<DeleteUserByIdCommandRequest>(request);
        applicationRequest.UserId = id;

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}