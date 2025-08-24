using Application.Handlers.Authentication;
using Application.Mappings;
using Base.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly))
			.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly)
			.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
		
		services.AddAutoMapper(_ => { }, typeof(DtosMappings).Assembly);

		return services;
	}
}