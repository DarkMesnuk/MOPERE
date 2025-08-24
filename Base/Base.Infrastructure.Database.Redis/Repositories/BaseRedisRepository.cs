using Base.Domain.Exceptions;
using Base.Domain.Infrastructure.Interfaces.Repositories.Redis;
using Base.Domain.Models.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Base.Infrastructure.Database.Redis.Repositories;

public class BaseRedisRepository<TModel>(
    IConnectionMultiplexer redis,
    string keyPart
) : IBaseRedisRepository<TModel>
    where TModel : class, IEntityModel, new()
{
    private readonly IDatabase _db = redis.GetDatabase();
    
    protected virtual TimeSpan Expiry => TimeSpan.FromHours(1);

    private string GetItemKey(string key) => keyPart + " - " + key;

    public virtual async Task<TModel> GetAsync(string key)
    {
        var entityJson = await _db.StringGetAsync(GetItemKey(key));

        if (entityJson.IsNullOrEmpty)
        {
            throw new NotFoundException(typeof(TModel).Name, GetItemKey(key));
        }
        
        var entity = JsonConvert.DeserializeObject<TModel>(entityJson!);

        if (entity == null)
        {
            throw new NotFoundException(typeof(TModel).Name, GetItemKey(key));
        }

        return entity;
    }

    public virtual async Task<TModel?> GetOrDefaultAsync(string key)
    {
        var entityJson = await _db.StringGetAsync(GetItemKey(key));

        if (entityJson.IsNullOrEmpty)
        {
            return null;
        }
        
        var entity = JsonConvert.DeserializeObject<TModel>(entityJson!);

        return entity;
    }

    public virtual Task UpdateOrCreateAsync(string key, TModel value, TimeSpan? expiry = null)
    {
        var json = JsonConvert.SerializeObject(value);
        
        expiry ??= Expiry;
        
        return _db.StringSetAsync(GetItemKey(key), json, expiry);
    }

    public virtual Task<bool> DeleteAsync(string key)
    {
        return _db.KeyDeleteAsync(GetItemKey(key));
    }
}