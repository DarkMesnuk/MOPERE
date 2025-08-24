using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Requests;

namespace Base.Domain.Extensions;

public static class CollectionsExtension
{
    public static void ExistsOrAdd<TEntity>(this ICollection<TEntity> collection, TEntity entity)
    {
        if (!collection.Contains(entity))
        {
            collection.Add(entity);	
        }
    }
    
    public static IEnumerable<TEntity> Page<TEntity>(this IEnumerable<TEntity> collection, IPaginatedRequest schema)
    {
        if (schema.PageNumber <= 0)
            return collection;
        
        return collection.Skip((schema.PageNumber - 1) * schema.PageSize)
            .Take(schema.PageSize);
    }
    
    public static PaginatedResponse<TEntity> ToPaginated<TEntity>(this IEnumerable<TEntity> paginatedCollection, IEnumerable<TEntity> collection)
    {
        var totalCount = collection.LongCount();

        var entities = paginatedCollection
            .ToList();

        var response = new PaginatedResponse<TEntity>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities
        };

        return response;
    }
}