using Base.Domain.Models.Interfaces;

namespace Base.Domain.Infrastructure.Interfaces.Repositories.Redis;

public interface IBaseRedisRepository<T>
    where T : class, IEntityModel, new()
{
    Task<T> GetAsync(string key);
    Task<T?> GetOrDefaultAsync(string key);
    Task UpdateOrCreateAsync(string key, T value, TimeSpan? expiry = null);
    Task<bool> DeleteAsync(string key);
}