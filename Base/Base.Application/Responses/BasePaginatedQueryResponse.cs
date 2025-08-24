using AutoMapper;
using Base.Application.Dtos.Interfaces;
using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;
using Base.Domain.Models.Interfaces;
using Base.Domain.Schemas.Interfaces.Responses;

namespace Base.Application.Responses;

public abstract class BasePaginatedQueryResponse<T, TDto, TModel>(
    IMapper mapper
) : BaseListQueryResponse<T, TDto, TModel>(mapper), IPaginatedResponse<TDto>
    where T : ApplicationResponse
    where TDto : IEntityDto, new()
    where TModel : IEntityModel, new()
{
    public long Count { get; set; }
    public long TotalCount { get; set; }

    public T SetData(IPaginatedResponse<TModel> schema)
    {
        SetData(StatusCodes.Success);

        Count = schema.Count;
        TotalCount = schema.TotalCount;
        Dtos = Mapper.Map<IEnumerable<TDto>>(schema.Dtos);

        return (this as T)!;
    }
}