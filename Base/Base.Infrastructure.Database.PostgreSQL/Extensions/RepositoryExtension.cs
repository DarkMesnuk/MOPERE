using System.Linq.Expressions;
using System.Reflection;
using Base.Domain.Entities.Interfaces;
using Base.Domain.Enums;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Requests;
using Microsoft.EntityFrameworkCore;

namespace Base.Infrastructure.Database.PostgreSQL.Extensions;

public static class RepositoryExtension
{
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
        var totalCount = await query.LongCountAsync(cancellationToken);
        
        var entities = await paginatedQuery.ToListAsync(cancellationToken);

        var response = new PaginatedResponse<TEntity>
        {
            TotalCount = totalCount,
            Count = entities.Count,
            Dtos = entities,
        };

        return response;
    }

    public static IQueryable<TEntity> FilterByIds<TEntity, TEntityType>(this IQueryable<TEntity> query, IEnumerable<TEntityType> ids)
        where TEntity : class, IEntity<TEntityType>, new()
    {
        return query.Where(e => ids.Contains(e.Id));
    }

    public static IQueryable<T> SortByTargetOrDefault<T>(this IQueryable<T> query, ISortedRequest schema,
        Dictionary<string, string>? propertyMapping = null)
    {
        return query
            .Sort(schema.Sort != null ? schema : new SortedByCreatedAtRequest());
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

        var sortDirectionString = schema.SortDirection == SortDirection.Asc ? "OrderBy" : "OrderByDescending";
        
        var orderByAsc = Expression.Call(typeof(Queryable), sortDirectionString, [typeof(T), propertyExpression.Type], query.Expression, lambda);
        return query.Provider.CreateQuery<T>(orderByAsc);
    }

    public static ModelBuilder EntityValueGeneratedOnAdd<E, ET>(this ModelBuilder builder)
        where E : class, IEntity<ET>, new()
    {
        builder.Entity<E>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
        
        return builder;
    }

    public static ModelBuilder EntityValueGeneratedOnAdd<E>(this ModelBuilder builder)
        where E : class, IEntity<string>, new()
    {
        builder.Entity<E>()
            .Property(entity => entity.Id)
            .ValueGeneratedOnAdd();
        
        return builder;
    }
    
    public static IQueryable<T> ApplyRenFilter<T, TValue>(
        this IQueryable<T> query,
        RenFilterSchema<TValue>? filter,
        Expression<Func<T, TValue>> selector)
        where TValue : struct, IComparable<TValue>
    {
        if (filter == null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        if (filter.From.HasValue)
        {
            var fromExpression = Expression.Lambda<Func<T, bool>>(
                Expression.GreaterThanOrEqual(
                    Expression.Invoke(selector, parameter),
                    Expression.Constant(filter.From.Value)
                ),
                parameter
            );
            query = query.Where(fromExpression);
        }

        if (filter.To.HasValue)
        {
            var toExpression = Expression.Lambda<Func<T, bool>>(
                Expression.LessThanOrEqual(
                    Expression.Invoke(selector, parameter),
                    Expression.Constant(filter.To.Value)
                ),
                parameter
            );
            query = query.Where(toExpression);
        }

        if (filter.Equal.HasValue)
        {
            var equalExpression = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Invoke(selector, parameter),
                    Expression.Constant(filter.Equal.Value)
                ),
                parameter
            );
            query = query.Where(equalExpression);
        }

        if (filter.NotEqual.HasValue)
        {
            var notEqualExpression = Expression.Lambda<Func<T, bool>>(
                Expression.NotEqual(
                    Expression.Invoke(selector, parameter),
                    Expression.Constant(filter.NotEqual.Value)
                ),
                parameter
            );
            query = query.Where(notEqualExpression);
        }

        return query;
    }
}