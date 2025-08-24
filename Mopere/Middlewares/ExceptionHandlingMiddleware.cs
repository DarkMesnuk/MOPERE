using System.Net;
using System.Reflection;
using System.Text.Json;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using StatusCodes = Base.Domain.Helpers.StatusCodes;

namespace Mopere.Middlewares;

public class ExceptionHandlingMiddleware(
	RequestDelegate next, 
	ILogger<ExceptionHandlingMiddleware> logger
)
{
	public virtual async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (MopereException ex)
		{
			logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" has failed", context.Request.GetDisplayUrl());

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)GetStatusCode(ex);
                
			await context.Response.WriteAsync(SerializeResponse(ex.ErrorResponse.GetErrorResult()));
		}
		catch (ValidationException ex)
		{
			logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" validation has failed", context.Request.GetDisplayUrl());

			var response = new ApplicationResponse().SetData(StatusCodes.ValidationFailed);
            
			foreach (var error in ex.Errors)
				response.SetFieldErrorMessage(error.PropertyName, error.ErrorMessage);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			await context.Response.WriteAsync(SerializeResponse(response.GetErrorResult()));
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" has failed with internal error", context.Request.GetDisplayUrl());

			var response = new ApplicationResponse().SetData(StatusCodes.ServerSomethingWentWrong, ex.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = response.HttpCode;

			await context.Response.WriteAsync(SerializeResponse(response.GetErrorResult()));
		}
	}
	
	private HttpStatusCode GetStatusCode(MopereException ex)
	{
		var statusCodeAttribute = ex.GetType().GetCustomAttribute<HttpStatusCodeAttribute>();
		return statusCodeAttribute?.StatusCode ?? HttpStatusCode.BadRequest;
	}

	private string SerializeResponse(ErrorResult response)
	{
		return JsonSerializer.Serialize(response, new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});
	}
}