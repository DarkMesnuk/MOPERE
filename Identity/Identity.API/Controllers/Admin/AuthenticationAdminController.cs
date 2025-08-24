using Identity.API.Requests.Authentication.Admin;
using Application.Handlers.Authentication.Admin;
using AutoMapper;
using Base.API.Controllers;
using Contract.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers.Admin;

[Route("api/v1/admin/authentication")]
public class AuthenticationAdminController(
    ILogger<AuthenticationAdminController> logger, 
    IMapper mapper, 
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// [Admin] Blocking user 
    /// </summary>
    [HttpPut]
    [Route("block")]
    [Authorize(Roles = RoleConsts.SuperAdmin)]
    public async Task<IActionResult> BlockingUser([FromBody] BlockingUserRequest request)
    {
        var applicationRequest = Mapper.Map<BlockingUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}