using AutoMapper;
using Base.Domain.Entities.Interfaces;
using Base.Domain.Exceptions;
using Base.Domain.Infrastructure.Interfaces.Repositories;
using Base.Domain.Models.Interfaces;
using Base.Domain.Schemas;
using Base.Domain.Schemas.Interfaces.Responses;
using Base.Infrastructure.Database.Mongo.Entities.Interfaces;
using Base.Infrastructure.Database.Mongo.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ReSharper disable PossibleMultipleEnumeration

namespace Base.Infrastructure.Database.Mongo.Repositories;

public abstract class BaseMongoRepository<TEntity, TModel, TId>(
    ILogger<BaseMongoRepository<TEntity, TModel, TId>> logger, 
    IMongoCollection<TEntity> collection, 
    IMapper mapper
) : IDisposable, IBaseRepository<TModel, TId>
    where TEntity : class, IMongoEntity<TId>, new()
    where TModel : class, IEntityModel<TId>, new()
{
    protected readonly IMongoCollection<TEntity> Collection = collection;
    
    protected TEntity MapToEntity(TModel model) => mapper.Map<TEntity>(model);

    protected IEnumerable<TEntity> MapToEntity(IEnumerable<TModel> models) => mapper.Map<IEnumerable<TEntity>>(models);

    protected TModel MapToModel(TEntity entity) => mapper.Map<TModel>(entity);

    protected IEnumerable<TModel> MapToModel(IEnumerable<TEntity> entities) => mapper.Map<IEnumerable<TModel>>(entities);

    protected List<TModel> MapToModel(List<TEntity> entities) => mapper.Map<List<TModel>>(entities);

    protected PaginatedResponse<TModel> MapToModel(IPaginatedResponse<TEntity> schema)
    {
        return new PaginatedResponse<TModel>
        {
            Dtos = MapToModel(schema.Dtos),
            Count = schema.Count,
            TotalCount = schema.TotalCount
        };
    }
    
    protected IQueryable<TEntity> GetBase => Collection.AsQueryable()
        .OfType<TEntity>();

    protected FilterDefinition<TEntity> GetFilterById(TId id)
    {
        return Builders<TEntity>.Filter.Eq(x => x.Id, id);
    }

    protected FilterDefinition<TEntity> GetFilterByIds(IEnumerable<TId> ids)
    {
        return Builders<TEntity>.Filter.In(x => x.Id, ids);
    }

    public bool Exists(TId id)
    {
        return Collection.Find(GetFilterById(id)).Any();
    }

    public Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return Collection.Find(GetFilterById(id)).AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsOrThrowAsync(TId id, CancellationToken cancellationToken = default)
    {
        var exists = await ExistsAsync(id, cancellationToken);
        
        if (!exists)
        {
            throw new NotFoundException(typeof(TModel).Name, id.ToString()!);
        }
        
        return exists;
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return Collection.Find(FilterDefinition<TEntity>.Empty).AnyAsync(cancellationToken);
    }

    public async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = MapToEntity(model);
            var options = new InsertOneOptions();

            SetDateTime(entity);

            await Collection.InsertOneAsync(entity, options, cancellationToken);
            
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
            var entities = MapToEntity(models);

            foreach (var entity in entities)
            {
                SetDateTime(entity);
            }

            await Collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range creations failed", typeof(TModel).Name);
            throw new CreationFailedException();
        }

        return true;
    }
    
    public virtual async Task<TModel?> GetByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetEntityByIdOrDefaultAsync(id, cancellationToken);
        
        if (entity.IsNullOrDefault())
             return null;
        
        return MapToModel(entity);
    }

    public virtual async Task<List<TModel>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        var entities = await GetEntitiesByIdsAsync(ids, cancellationToken);
        
        return MapToModel(entities);
    }

    public virtual async Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetEntityByIdOrDefaultAsync(id, cancellationToken);
        
        if (entity.IsNullOrDefault())
            throw new NotFoundException(typeof(TModel).Name, id.ToString()!);
        
        return MapToModel(entity);
    }
    
    protected Task<TEntity> GetEntityByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default)
    {
        return Collection.Find(GetFilterById(id)).FirstOrDefaultAsync(cancellationToken);
    }
    
    protected Task<List<TEntity>> GetEntitiesByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        return Collection.Find(GetFilterByIds(ids)).ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = MapToEntity(model);
            
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            UpdateDateTime(entity);
            await Collection.FindOneAndReplaceAsync(filter, entity, cancellationToken: cancellationToken);

            model = MapToModel(entity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} update failed", typeof(TModel).Name);
            throw new UpdateFailedException();
        }

        return model;
    }
    
    public async Task<bool> UpdateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = MapToEntity(models);

            foreach (var entity in entities)
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
                UpdateDateTime(entity);
                await Collection.FindOneAndReplaceAsync(filter!, entity, cancellationToken: cancellationToken);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range update failed", typeof(TModel).Name);
            throw new UpdateFailedException();
        }

        return true;
    }
    
    protected void UpdateOne(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
    {
        Collection.UpdateOne(filter, update);
    }

    protected Task UpdateOneAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, CancellationToken cancellationToken = default)
    {
        return Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var entity = await GetEntityByIdOrDefaultAsync(id, cancellationToken);
            var isSetDeletedState = SetDeleteState(entity);

            if (isSetDeletedState)
            {
                var updateOption = Builders<TEntity>.Update
                    .Set(nameof(IEntityDeleted.IsDeleted), true);

                var result = await Collection.UpdateOneAsync(GetFilterById(id), updateOption, cancellationToken: cancellationToken);
             
                return result is { IsAcknowledged: true, ModifiedCount: 1 }; 
            }
            else
            {
                var options = new DeleteOptions();
                var result = await Collection.DeleteOneAsync(GetFilterById(id), options, cancellationToken: cancellationToken);
            
                return result is { IsAcknowledged: true, DeletedCount: 1 }; 
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range delete failed", typeof(TModel).Name);
            throw new DeleteFailedException();
        }
    }
    
    public Task<bool> DeleteAsync(TModel model, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(model.Id, cancellationToken);
    }
    
    public Task<bool> DeleteAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        var entities = MapToEntity(models);
        return DeleteRangeAsync(entities, cancellationToken);
    }

    private Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var ids = entities.Select(x => x.Id);
        return DeleteAsync(ids, cancellationToken);
    }
    
    public async Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await GetEntitiesByIdsAsync(ids, cancellationToken);
            
            var isSetDeletedState = SetDeleteState(entities);

            if (isSetDeletedState)
            {
                var updateOption = Builders<TEntity>.Update
                    .Set(nameof(IEntityDeleted.IsDeleted), true);

                var result = await Collection.UpdateManyAsync(GetFilterByIds(ids), updateOption, cancellationToken: cancellationToken);
            
                return result is { IsAcknowledged: true } && result.ModifiedCount == ids.Count(); 
            }
            else
            {
                var options = new DeleteOptions();
                var result = await Collection.DeleteManyAsync(GetFilterByIds(ids), options, cancellationToken: cancellationToken);
            
                return result is { IsAcknowledged: true } && result.DeletedCount == ids.Count(); 
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Name} range delete failed", typeof(TModel).Name);
            throw new DeleteFailedException();
        }
    }
    
    protected void SetDateTime(TEntity entity)
    {
        if (entity is not IEntityDateTime entityDateTime)
            return;

        entityDateTime.CreatedAt = DateTime.UtcNow;
        entityDateTime.ModifiedAt = DateTime.UtcNow;
    }
    
    protected void UpdateDateTime(TEntity entity)
    {
        if (entity is not IEntityDateTime entityDateTime)
            return;

        entityDateTime.ModifiedAt = DateTime.UtcNow;
    }
    
    protected bool SetDeleteState(IEnumerable<TEntity> entities)
    {
        var isSetDeletedState = false;
        
        foreach (var entity in entities)
            isSetDeletedState = SetDeleteState(entity);
        
        return isSetDeletedState;
    }

    protected bool SetDeleteState(TEntity entity)
    {
        if (entity is IEntityDeleted entityDelete)
        {
            entityDelete.IsDeleted = true;
            return true;
        }
        else
        {
            return false;   
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}