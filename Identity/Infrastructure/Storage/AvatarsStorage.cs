using Base.Infrastructure.Storage;
using Base.Infrastructure.Storage.Configs;
using Domain.Interfaces.Infrastructure.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Storage;

public class AvatarsStorage(
    IOptions<StorageConfigurations> storageConfigurationsOptions,
    ILogger<AvatarsStorage> logger
) : BaseStorage(storageConfigurationsOptions, logger), IAvatarsStorage
{
    private const string BucketName = "mopere-identity-dev-assets";
    private const string FolderPath = "avatars";

    public async Task<string?> UploadAsync(IFormFile file, string key, CancellationToken cancellationToken = default)
    {
        var fileName = $"ava_{key}.{file.ContentType.Split('/')[1]}";
        return await UploadToBucket(file, fileName, BucketName, FolderPath, cancellationToken: cancellationToken);
    }

    public Task<bool> DeleteByUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        return DeleteFileOrFolder(url, cancellationToken);
    }

    public bool IsCorrectFileType(IFormFile file)
    {
        return base.IsCorrectImageType(file);
    }

    public new bool IsCorrectImageType(IFormFile file)
    {
        return base.IsCorrectImageType(file);
    }
}