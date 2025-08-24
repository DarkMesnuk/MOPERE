using System.Linq.Expressions;
using System.Reflection;
using Base.Domain.Entities.Interfaces;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Requests;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Base.Infrastructure.Database.Mongo.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddMongoCollection<TCollection>(this IServiceCollection collection, string collectionName, MongoCollectionSettings? settings = null)
    {
        collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));

        settings ??= new MongoCollectionSettings();
        settings.WriteConcern = WriteConcern.Acknowledged;
        settings.AssignIdOnInsert = true;

        if (!BsonClassMap.IsClassMapRegistered(typeof(TCollection)))
        {
            BsonClassMap.RegisterClassMap<TCollection>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIsRootClass(true);
            });
        }

        return collection.AddScoped<IMongoCollection<TCollection>>(provider =>
        {
            var database = provider.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<TCollection>(collectionName, settings);
        });
    }
    
    public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> query, IPaginatedRequest schema)
        where TEntity : class, IEntity, new()
    {
        if (schema.PageNumber <= 0)
            return query;
        
        return query.Skip((schema.PageNumber - 1) * schema.PageSize)
            .Take(schema.PageSize);
    }
    
    public static PaginatedResponse<TEntity> ToPaginated<TEntity>(this IQueryable<TEntity> paginatedQuery, IQueryable<TEntity> query)
        where TEntity : class, IEntity, new()
    {
        var totalCount = query.LongCount();

        var entities = paginatedQuery
            .ToList();

        var response = new PaginatedResponse<TEntity>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities
        };

        return response;
    }
    
    public static async Task<PaginatedResponse<TEntity>> ToPaginatedAsync<TEntity>(this IQueryable<TEntity> paginatedQuery, IQueryable<TEntity> query, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        var totalCount = await query.LongCountAsync(cancellationToken: cancellationToken);
        
        var entities = await paginatedQuery.ToListAsync(cancellationToken: cancellationToken);

        var response = new PaginatedResponse<TEntity>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities,
        };

        return response;
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query, ISortedRequest schema, Dictionary<string, string>? propertyMapping = null)
    {
        if (string.IsNullOrWhiteSpace(schema.Sort))
            return query;

        var sortPropertyPath = schema.Sort;

        if (propertyMapping != null)
        {
            var mappedProperty = propertyMapping.GetValueOrDefault(schema.Sort.ToLower());
            
            if (!string.IsNullOrEmpty(mappedProperty))
                sortPropertyPath = mappedProperty;
        }

        var properties = sortPropertyPath.Split('.');
        var parameter = Expression.Parameter(typeof(T), "x");

        Expression propertyExpression = parameter;
        
        foreach (var prop in properties)
        {
            var propertyInfo = propertyExpression.Type.GetProperty(prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            
            if (propertyInfo == null)
                return query;

            propertyExpression = Expression.Property(propertyExpression, propertyInfo);
        }

        var lambda = Expression.Lambda(propertyExpression, parameter);

        var sortDirectionString = schema.SortDirection == Domain.Enums.SortDirection.Asc ? "OrderBy" : "OrderByDescending";
        
        var orderByAsc = Expression.Call(typeof(Queryable), sortDirectionString, [typeof(T), propertyExpression.Type], query.Expression, lambda);
        return query.Provider.CreateQuery<T>(orderByAsc);
    }
}