using Identity.API.Requests.Claims;
using Identity.API.Responses.Claims;
using Application.Handlers.Claims;
using AutoMapper;
using Base.API.Controllers;
using Base.API.Responses;
using Contract.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers.Admin;

[Authorize(Roles = RoleConsts.SuperAdmin)]
[Route("api/v1/admin/claim")]
public class ClaimsAdminController(
    ILogger<ClaimsAdminController> logger, 
    IMapper mapper,
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// [Admin] Create claim
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimRequest request)
    {
        var applicationRequest = Mapper.Map<CreateClaimCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Get claims
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ClaimsPaginatedResponse), 200)]
    public async Task<IActionResult> Get([FromQuery] GetClaimsRequest request)
    {
        var applicationRequest = Mapper.Map<GetClaimsQueryRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new ClaimsPaginatedResponse(applicationResponse);
        
        return Ok(response);
    }
    
    /// <summary>
    /// [Admin] Update claim by claimId 
    /// </summary>
    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateClaimRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UpdateClaimCommandRequest>(request);
        applicationRequest.Id = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Delete claim by claimId 
    /// </summary>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromQuery] DeleteClaimRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<DeleteClaimCommandRequest>(request);
        applicationRequest.Id = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Is user have claim 
    /// </summary>
    [HttpGet]
    [Route("is-user-have")]
    [ProducesResponseType(typeof(BoolResponse), 200)]
    public async Task<IActionResult> IsUserHave([FromQuery] IsUserHaveClaimRequest request)
    {
        var applicationRequest = Mapper.Map<IsUserHaveClaimCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();
        
        var response = new BoolResponse(applicationResponse.Dto);
        
        return Ok(response);
    }
    
    /// <summary>
    /// [Admin] Set claims to user 
    /// </summary>
    [HttpPut]
    [Route("set-to-user")]
    public async Task<IActionResult> SetToUser([FromBody] SetClaimsToUserRequest request)
    {
        var applicationRequest = Mapper.Map<SetClaimsToUserCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Assign claim to user 
    /// </summary>
    [HttpPut]
    [Route("{id}/assign-to-user")]
    public async Task<IActionResult> AssignToUser([FromBody] AssignClaimToUserRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<AssignClaimToUserCommandRequest>(request);
        applicationRequest.ClaimId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign claim from user 
    /// </summary>
    [HttpPut]
    [Route("{id}/unassign-from-user")]
    public async Task<IActionResult> UnassignFromUser([FromBody] UnassignClaimFromUserRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UnassignClaimFromUserCommandRequest>(request);
        applicationRequest.ClaimId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign all claims from user 
    /// </summary>
    [HttpPut]
    [Route("unassign-from-user")]
    public async Task<IActionResult> UnassignAllFromUser([FromBody] UnassignAllClaimsFromUserRequest request)
    {
        var applicationRequest = Mapper.Map<UnassignAllClaimsFromUserCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Set claims to role
    /// </summary>
    [HttpPut]
    [Route("set-to-role")]
    public async Task<IActionResult> SetToRole([FromBody] SetClaimsToRoleRequest request)
    {
        var applicationRequest = Mapper.Map<SetClaimsToRoleCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Assign claim to role 
    /// </summary>
    [HttpPut]
    [Route("{id}/assign-to-role")]
    public async Task<IActionResult> AssignToRole([FromBody] AssignClaimToRoleRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<AssignClaimToRoleCommandRequest>(request);
        applicationRequest.ClaimId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign claim from role 
    /// </summary>
    [HttpPut]
    [Route("{id}/unassign-from-role")]
    public async Task<IActionResult> UnassignFromRole([FromBody] UnassignClaimFromRoleRequest request, [FromRoute] string id)
    {
        var applicationRequest = Mapper.Map<UnassignClaimFromRoleCommandRequest>(request);
        applicationRequest.ClaimId = id;
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// [Admin] Unassign all claims from role 
    /// </summary>
    [HttpPut]
    [Route("unassign-from-role")]
    public async Task<IActionResult> UnassignAllFromRole([FromBody] UnassignAllClaimsFromRoleRequest request)
    {
        var applicationRequest = Mapper.Map<UnassignAllClaimsFromRoleCommandRequest>(request);
        
        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}