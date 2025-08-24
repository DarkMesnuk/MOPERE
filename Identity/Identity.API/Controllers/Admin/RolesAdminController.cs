using Identity.API.Requests.Roles;
using Identity.API.Responses.Roles;
using Application.Handlers.Roles;
using AutoMapper;
using Base.API.Controllers;
using Contract.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers.Admin;

[Authorize(Roles = RoleConsts.SuperAdmin)]
[Route("api/v1/admin/role")]
public class RolesAdminController(
    ILogger<RolesAdminController> logger, 
    IMapper mapper,
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// [Admin] Create role
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var applicationRequest = Mapper.Map<CreateRoleCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Get roles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(RolesResponse), 200)]
    public async Task<IActionResult> Get([FromQuery] GetRolesRequest request)
    {
        var applicationRequest = Mapper.Map<GetRolesCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new RolesResponse(applicationResponse);
        
        return Ok(response);
    }
    
    /// <summary>
    /// [Admin] Update role by roleId 
    /// </summary>
    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateRoleRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UpdateRoleCommandRequest>(request);
        applicationRequest.Id = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Delete role by roleId 
    /// </summary>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] DeleteRoleRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<DeleteRoleCommandRequest>(request);
        applicationRequest.Id = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Assign role to user 
    /// </summary>
    [HttpPut]
    [Route("{id}/assign-to-user")]
    public async Task<IActionResult> AssignToUser([FromBody] AssignRoleToUserRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<AssignRoleToUserCommandRequest>(request);
        applicationRequest.RoleId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign role from user 
    /// </summary>
    [HttpPut]
    [Route("{id}/unassign-from-user")]
    public async Task<IActionResult> UnassignFromUser([FromBody] UnassignRoleFromUserRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UnassignRoleFromUserCommandRequest>(request);
        applicationRequest.RoleId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign all roles from user 
    /// </summary>
    [HttpPut]
    [Route("unassign-from-user")]
    public async Task<IActionResult> UnassignAllFromUser([FromBody] UnassignAllRolesFromUserRequest request)
    {
        var applicationRequest = Mapper.Map<UnassignAllRolesFromUserCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}