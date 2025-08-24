using AutoMapper;
using Base.Application.Dtos.Interfaces;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Base.Domain.Models.Interfaces;

namespace Base.Application.Responses;

public abstract class BaseListQueryResponse<T, TDto, TModel> : ApplicationResponse<T>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{
    protected readonly IMapper Mapper;

    protected BaseListQueryResponse(IMapper mapper)
    {
        Mapper = mapper;
    }

    public IEnumerable<TDto> Dtos { get; set; }

    public T SetData(IEnumerable<TModel> model)
    {
        SetData(StatusCodes.Success);

        Dtos = Mapper.Map<IEnumerable<TDto>>(model);

        return (this as T)!;
    }
}