using Identity.API.Requests.Users;
using Identity.API.Responses.Users;
using Application.Dtos.Users;
using Application.Handlers.Users;
using AutoMapper;
using Base.API.Controllers;
using Base.API.Responses;
using Contract.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers;

[Authorize(Roles = RoleConsts.User)]
[Route("api/v1/user")]
public class UsersController(
    ILogger<UsersController> logger, 
    IMapper mapper, 
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// Get user
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), 200)]
    public async Task<IActionResult> Get([FromQuery] GetUserRequest request)
    {
        var applicationRequest = Mapper.Map<GetUserQueryRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new UserResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Get users
    /// </summary>
    [HttpGet]
    [Route("all")]
    [ProducesResponseType(typeof(UsersPaginatedResponse), 200)]
    public async Task<IActionResult> Get([FromQuery] GetUsersRequest request)
    {
        var applicationRequest = Mapper.Map<GetUsersQueryRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new UsersPaginatedResponse(applicationResponse);

        return Ok(response);
    }
    
    /// <summary>
    /// Update user 
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
    {
        var applicationRequest = Mapper.Map<UpdateUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// Send code to delete user by userId 
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> Delete([FromQuery] DeleteUserRequest request)
    {
        var applicationRequest = Mapper.Map<DeleteUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Confirm delete user by userId 
    /// </summary>
    [HttpDelete]
    [Route("confirm")]
    public async Task<IActionResult> ConfirmDelete([FromQuery] ConfirmDeleteUserRequest request)
    {
        var applicationRequest = Mapper.Map<ConfirmDeleteUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}