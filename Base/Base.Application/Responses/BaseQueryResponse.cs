using AutoMapper;
using Base.Application.Dtos.Interfaces;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Base.Domain.Models.Interfaces;

namespace Base.Application.Responses;

public abstract class BaseQueryResponse<T, TDto, TModel>(IMapper mapper) : ApplicationResponse<T>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{
    public TDto Dto { get; set; }
    
    public T SetData(TModel model)
    {
        SetData(StatusCodes.Success);
        
        Dto = mapper.Map<TDto>(model);

        return (this as T)!;
    }
}

public abstract class BaseQueryResponse<T, TDto> : ApplicationResponse<T>
    where T : ApplicationResponse
{
    public TDto Dto { get; set; }

    public T SetData(TDto dto)
    {
        SetData(StatusCodes.Success);
        Dto = dto;
        return (this as T)!;
    }
}