using Base.Domain.Models.Interfaces;

namespace Base.Domain.Infrastructure.Interfaces.Repositories;

public interface IBaseRepository<TModel, TId>
    where TModel : class, IEntityModel<TId>, new()
{
    bool Exists(TId id);
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsOrThrowAsync(TId id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    
    Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
    
    Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<TModel?> GetByIdOrDefaultAsync(TId id, CancellationToken cancellationToken = default);
    
    Task<List<TModel>> GetByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
    
    Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TModel model, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default);
}