using AutoMapper;
using Base.API.Requests;
using Base.Application.Requests;

namespace Base.API.Extensions;

public static class MapperExtensions
{
    public static IMappingExpression<TSource, TDestination> CreateAuthMap<TSource, TDestination>(this Profile profile)
        where TSource : BaseAuthRequest
        where TDestination : BaseAuthHandlerRequest
    {
        return profile.CreateMap<TSource, TDestination>()
            .ForMember(x => x.AuthUserId, expression => expression.MapFrom(x => x.GetActiveUserId()));
    }
}