using System.Security.Claims;
using Base.API.Extensions;
using Base.API.Requests;
using Base.Domain.Chekers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mopere.Middlewares.Filters;

public class TokenAssignmentActionFilter(
    IHttpContextAccessor httpContextAccessor, 
    IIdentityAccessChecker identityService
) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (httpContextAccessor.HttpContext != null)
        {
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            var request = context.ActionArguments.Values.FirstOrDefault();
        
            await SetDataToAuthRequest(request, claimsPrincipal, identityService.ThrowIfUserNotPossibleToAuthAsync);
        }
        
        await next();
    }

    private async Task SetDataToAuthRequest(object? request, ClaimsPrincipal claimsPrincipal, Func<string?, Task> throwIfUserNotPossibleToAuth)
    {
        if (request is BaseAuthRequest authRequest)
        {
            var userId = claimsPrincipal.GetActiveUserId();

            await throwIfUserNotPossibleToAuth(userId);
            
            authRequest.SetActiveUserId(userId!);
        }
    }
}