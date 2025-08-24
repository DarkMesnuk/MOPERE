using System.Linq.Expressions;
using AutoMapper;
using Base.Domain.Entities.Interfaces;
using Base.Domain.Exceptions;
using Base.Domain.Infrastructure.Interfaces.Repositories;
using Base.Domain.Models.Interfaces;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Responses;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Base.Infrastructure.Database.PostgreSQL.Repositories;

public abstract class BaseRepository<TEntity, TModel, TId>(
    DbContext context, 
    ILogger<BaseRepository<TEntity, TModel, TId>> logger, 
    IMapper mapper
) : IDisposable, IBaseRepository<TModel, TId>
    where TEntity : class, IEntity<TId>, new()
    where TModel : class, IEntityModel<TId>, new()
{
    protected DbContext Context => context;
    
    protected IMapper Mapper => mapper;

    protected TEntity MapToEntity(TModel? model) => mapper.Map<TEntity>(model);
    
    protected IEnumerable<TEntity> MapToEntity(IEnumerable<TModel>? models) => mapper.Map<IEnumerable<TEntity>>(models);
    
    protected TModel MapToModel(TEntity? entity) => mapper.Map<TModel>(entity);
    
    protected IEnumerable<TModel> MapToModel(IEnumerable<TEntity>? entities) => mapper.Map<IEnumerable<TModel>>(entities);
    protected List<TModel> MapToModel(List<TEntity>? entities) => mapper.Map<List<TModel>>(entities);
    
    protected PaginatedResponse<TModel> MapToModel(IPaginatedResponse<TEntity> schema)
    {
        return new PaginatedResponse<TModel>
        {
            Dtos = MapToModel(schema.Dtos),
            Count = schema.Count,
            TotalCount = schema.TotalCount
        };
    }

    protected DbSet<TEntity> DbSet => context.Set<TEntity>();

    protected virtual IQueryable<TEntity> GetBase => DbSet.AsQueryable()
        .AsNoTracking();

    public bool Exists(TId id)
    {
        return GetBase.Any(x => x.Id.Equals(id));
    }

    public Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return GetBase.AnyAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<bool> ExistsOrThrowAsync(TId id, CancellationToken cancellationToken = default)
    {
        var exists = await GetBase.AnyAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);

        if (!exists)
        {
            throw new NotFoundException(typeof(TEntity).Name, id.ToString()!);
        }
        
        return exists;
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return GetBase.AnyAsync(cancellationToken);
    }

    protected bool Exists(Expression<Func<TEntity, bool>> func)
    {
        return GetBase.Any(func);
    }

    protected Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> func, CancellationToken cancellationToken = default)
    {
        return GetBase.AnyAsync(func, cancellationToken);
    }
    
    public async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = MapToEntity(model);
            var query = DbSet.Add(entity);

            entity = query.Entity;
            
            await context.SaveChangesAsync(cancellationToken);
            
            context.Entry(entity).State = EntityState.Detached;
            
            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} create failed", typeof(TModel).Name);
            throw new CreationFailedException();
        }

        return model;
    }
    
    public async Task<bool> CreateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = MapToEntity(models).ToList();

            DbSet.AddRange(entities);
            
            await context.SaveChangesAsync(cancellationToken);
            
            foreach (var entity in entities)
                context.Entry(entity).State = EntityState.Detached;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range creations failed", typeof(TModel).Name);
            throw new CreationFailedException();
        }

        return true;
    }
    
    protected async Task<bool> CreateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            DbSet.AddRange(entities);
            
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range creations failed", typeof(TModel).Name);
            throw new CreationFailedException();
        }

        return true;
    }
    
    public virtual async Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetBase.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString()!);

        return MapToModel(entity);
    }
    
    public virtual async Task<TModel?> GetByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetBase.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

        if (entity.IsNullOrDefault())
            return null;

        return MapToModel(entity);
    }

    public async Task<List<TModel>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        var entities = await GetBase
            .Where(x => ids.Any(id => x.Id.Equals(id)))
            .ToListAsync(cancellationToken: cancellationToken);
        
        if (entities.Count != ids.Count())
            throw new NotFoundException(typeof(TModel).Name);

        return MapToModel(entities);
    }

    public async Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = MapToEntity(model);
            
            SetModifiedState(entity);

            var query = DbSet.Update(entity);

            entity = query.Entity;

            await context.SaveChangesAsync(cancellationToken);

            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} update failed", typeof(TModel).Name);
            throw new UpdateFailedException();
        }

        return model;
    }

    protected async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            SetModifiedState(entity);

            var query = DbSet.Update(entity);

            entity = query.Entity;

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} update failed", typeof(TModel).Name);
            throw new UpdateFailedException();
        }

        return entity;
    }

    public async Task<bool> UpdateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = MapToEntity(models).ToList();
            
            SetModifiedState(entities);

            DbSet.UpdateRange(entities);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range update failed", typeof(TModel).Name);
            throw new UpdateFailedException();
        }

        return true;
    }

    public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetBase.FirstOrDefaultAsync(x => x.Id.Equals(id));
        
        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString()!);
        
        return await DeleteAsync(entity!, cancellationToken);
    }

    public Task<bool> DeleteAsync(TModel model, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(MapToEntity(model), cancellationToken);
    }
    
    protected async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> func, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = GetBase.Where(func);

            await entities.ExecuteDeleteAsync(cancellationToken);
            
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range delete failed", typeof(TModel).Name);
            throw new DeleteFailedException();
        }

        return true;
    }

    public async Task<bool> DeleteAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = MapToEntity(models).ToList();
            
            SetDeleteState(entities);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range delete failed", typeof(TModel).Name);
            throw new DeleteFailedException();
        }

        return true;
    }
    
    public async Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = GetBase.FilterByIds(ids);

            SetDeleteState(entities);
            
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range delete failed", typeof(TModel).Name);
            throw new DeleteFailedException();
        }

        return true;
    }

    private async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            SetDeleteState(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{EntityName}.Id {EntityId} delete failed", nameof(TEntity), entity.Id);
            throw new DeleteFailedException();
        }

        return true;
    }
    
    protected void SetModifiedState(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetModifiedState(entity);
    }

    protected void SetModifiedState(TEntity entity)
    {
        SetDetachedState(entity);
        
        DbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }
    
    protected void SetDeleteState(IQueryable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetDeleteState(entity);
    }

    protected void SetDetachedState(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            SetDeleteState(entity);
    }
    
    protected void SetDetachedState(TEntity entity)
    {
        var existingEntity = context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Id.Equals(entity.Id));
        if (existingEntity != null)
            context.Entry(existingEntity.Entity).State = EntityState.Detached;
    }
    
    protected void SetDeleteState(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SetDeleteState(entity);
    }

    protected void SetDeleteState(TEntity entity)
    {
        SetDetachedState(entity);
        
        if (entity is IEntityDeleted entityDelete)
        {
            entityDelete.IsDeleted = true;
            context.Entry(entity).State = EntityState.Modified;
        }
        else
            context.Entry(entity).State = EntityState.Deleted;
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}