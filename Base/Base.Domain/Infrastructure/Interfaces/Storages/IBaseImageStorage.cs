using Microsoft.AspNetCore.Http;

namespace Base.Domain.Infrastructure.Interfaces.Storages;

public interface IBaseImageStorage
{
    Task<string?> UploadAsync(IFormFile file, string key, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUrlAsync(string url, CancellationToken cancellationToken = default);
    bool IsCorrectImageType(IFormFile file);
}