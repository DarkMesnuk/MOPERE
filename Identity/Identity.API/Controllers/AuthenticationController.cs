using Identity.API.Requests.Authentication;
using Identity.API.Responses.Authentication;
using Application.Handlers.Authentication;
using AutoMapper;
using Base.API.Controllers;
using Base.API.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers;

[Route("api/v1/authentication")]
public class AuthenticationController(
    ILogger<AuthenticationController> logger, 
    IMapper mapper, 
    IMediator mediator
) : BaseApiController(logger, mapper, mediator)
{
    /// <summary>
    /// Register
    /// </summary>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var applicationRequest = Mapper.Map<RegisterCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Confirm register
    /// </summary>
    [HttpPost]
    [Route("register/confirm")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationRequest request)
    {
        var applicationRequest = Mapper.Map<ConfirmRegistrationCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new TokenResponse(applicationResponse.Dto);

        return Ok(response);
    }

    /// <summary>
    /// Login by email
    /// </summary>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var applicationRequest = Mapper.Map<LoginCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new TokenResponse(applicationResponse.Dto);

        return Ok(response);
    }

    /// <summary>
    /// Refresh tokens
    /// </summary>
    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var applicationRequest = Mapper.Map<RefreshTokenCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new TokenResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Resent verification code
    /// </summary>
    [HttpPost]
    [Route("resend-code")]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest request)
    {
        var applicationRequest = Mapper.Map<ResendCodeCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Is valid verification code
    /// </summary>
    [HttpPost]
    [Route("is-valid-code")]
    public async Task<IActionResult> IsValidCode([FromBody] IsValidCodeRequest request)
    {
        var applicationRequest = Mapper.Map<IsValidCodeCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }

    /// <summary>
    /// Forgot password
    /// </summary>
    [HttpPost]
    [Route("forgot-password")]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var applicationRequest = Mapper.Map<ForgotPasswordCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }

    /// <summary>
    /// Confirm forgot password
    /// </summary>
    [HttpPost]
    [Route("forgot-password/confirm")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var applicationRequest = Mapper.Map<ResetPasswordCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }

    /// <summary>
    /// [Auth] Change email 
    /// </summary>
    [HttpPost]
    [Route("change-email")]
    [Authorize]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
    {
        var applicationRequest = Mapper.Map<ChangeEmailCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }

    /// <summary>
    /// [Auth] Confirm change email 
    /// </summary>
    [HttpPost]
    [Route("change-email/confirm")]
    [Authorize]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailRequest request)
    {
        var applicationRequest = Mapper.Map<ConfirmChangeEmailCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }

    /// <summary>
    /// [Auth] Confirm change email 
    /// </summary>
    [HttpPost]
    [Route("change-email/new/confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmNewEmail([FromBody] ConfirmNewEmailRequest request)
    {
        var applicationRequest = Mapper.Map<ConfirmNewEmailCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }

    /// <summary>
    /// [Auth] Change password 
    /// </summary>
    [HttpPost]
    [Route("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var applicationRequest = Mapper.Map<ChangePasswordCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
    
    /// <summary>
    /// Restore account   
    /// </summary>
    [HttpPost]
    [Route("restore")]
    [ProducesResponseType(typeof(StringResponse), 200)]
    public async Task<IActionResult> Restore([FromBody] RestoreUserRequest request)
    {
        var applicationRequest = Mapper.Map<RestoreUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);
        
        if (!applicationResponse.IsSucceeded)
            return applicationResponse.GetActionResult();

        var response = new StringResponse(applicationResponse.Dto);

        return Ok(response);
    }
    
    /// <summary>
    /// Confirm restore account   
    /// </summary>
    [HttpPost]
    [Route("restore/confirm")]
    public async Task<IActionResult> ConfirmRestoreUser([FromBody] ConfirmRestoreUserRequest request)
    {
        var applicationRequest = Mapper.Map<ConfirmRestoreUserCommandRequest>(request);

        var applicationResponse = await Mediator.Send(applicationRequest);

        return applicationResponse.IsSucceeded ? Ok() : applicationResponse.GetActionResult();
    }
}