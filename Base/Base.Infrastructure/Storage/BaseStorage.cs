using System.Diagnostics;
using System.Text.RegularExpressions;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Base.Infrastructure.Storage.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Base.Infrastructure.Storage;

public class BaseStorage(
    IOptions<StorageConfigurations> storageConfigurationsOptions,
    ILogger<BaseStorage> logger
)
{
    private readonly StorageConfigurations _storageConfigurations = storageConfigurationsOptions.Value;

    protected async Task<string?> UploadToBucket(IFormFile file, string fileName, string bucketName, string folderPath, string? cloudFrontPath = null, CancellationToken cancellationToken = default)
    {
        await using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream, cancellationToken);
        
        return await UploadToBucket(newMemoryStream, fileName, bucketName, S3CannedACL.PublicRead, folderPath, cloudFrontPath, cancellationToken);
    }

    protected async Task<string?> UploadToBucket(byte[] file, string fileName, string bucketName, string folderPath, string? cloudFrontPath = null, CancellationToken cancellationToken = default)
    {
        await using var newMemoryStream = new MemoryStream(file);
        
        return await UploadToBucket(newMemoryStream, fileName, bucketName, S3CannedACL.PublicRead, folderPath, cloudFrontPath, cancellationToken);
    }

    protected async Task<List<string>> ListFiles(string bucketName, string folderPath)
    {
        folderPath = ProcessFolderPath(folderPath);
        
        using var client = CreateS3Client();
        
        var request = new ListObjectsRequest
        {
            BucketName = bucketName,
            Prefix = folderPath
        };

        var files = new List<string>();
        
        try
        {
            var response = await client.ListObjectsAsync(request);

            files.AddRange(response.S3Objects.Select(obj => obj.Key));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AwsStorageService.ListFiles]: {ex.Message}. Inner exception: {ex.InnerException}");
        }
        
        return files;
    }

    protected Task<bool> DeleteFileOrFolder(string bucketName, string folderPath, string fileName, CancellationToken cancellationToken = default)
    {
        folderPath = ProcessFolderPath(folderPath);
        var key = $"{folderPath}{fileName}";

        return DeleteFile(bucketName, key, cancellationToken);
    }

    protected Task<bool> DeleteFileOrFolder(string url, CancellationToken cancellationToken = default)
    {
        var bucketName = GetBucketName(url);
        var key = GetFileKey(url);

        return DeleteFile(bucketName, key, cancellationToken);
    }

    protected async Task<int> DeleteFileOrFolder(IList<string> urls)
    {
        var count = 0;
        
        foreach (var url in urls)
        {
            var success = await DeleteFileOrFolder(url);
            
            if (success)
            {
                count++;
            }
        }
        
        return count;
    }
    
    protected bool IsCorrectImageType(IFormFile file)
    {
        var allowedTypes = new [] { "image/png", "image/jpeg" };
        return allowedTypes.Contains(file.ContentType);
    }

    protected bool IsCorrectImageType(string type)
    {
        var allowedTypes = new [] { "png", "jpeg", "jpg" };
        return allowedTypes.Contains(type);
    }

    protected string GetFolderUrl(string bucketName, string folderPath)
    {
        return _getBucketUrl(bucketName) + "/" + folderPath;
    }

    private string _getBucketUrl(string bucketName)
    {
        return _storageConfigurations.S3PathTemplate.Replace("[bucket_name]", bucketName);
    }

    private AmazonS3Client CreateS3Client()
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.USWest1
        };
        
        var credentials = new BasicAWSCredentials(_storageConfigurations.AWSAccessKey, _storageConfigurations.AWSSecretKey);
        
        return new AmazonS3Client(credentials, config);
    }

    private string ProcessFolderPath(string folderPath)
    {
        return string.IsNullOrEmpty(folderPath) ? string.Empty : folderPath[folderPath.Length - 1] != '/' ? $"{folderPath}/" : folderPath;
    }

    private string GetFileKey(string url)
    {
        var key = "";
        var pattern = @"https://(.*).s3(.*).com/([^?]*)";
        var rg = new Regex(pattern, RegexOptions.IgnoreCase);
        
        var matches = rg.Matches(url);
        
        if (matches.Count > 0 && matches[0].Groups.Count >= 4)
        {
            key = matches[0].Groups[3].Value;
        }
        
        return key;
    }

    private string GetBucketName(string url)
    {
        var key = "";
        var pattern = @"https://(.*).s3(.*).com/([^?]*)";
        var rg = new Regex(pattern, RegexOptions.IgnoreCase);
        
        var matches = rg.Matches(url);
        
        if (matches.Count > 0 && matches[0].Groups.Count >= 2)
        {
            key = matches[0].Groups[1].Value;
        }
        
        return key;
    }

    private async Task<string?> UploadToBucket(MemoryStream newMemoryStream, string fileName, string bucketName, S3CannedACL accessLevel, string? folderPath = null, string? cloudFrontPath = null, CancellationToken cancellationToken = default)
    {
        string? fileUrl = null;
        
        if (!string.IsNullOrEmpty(folderPath))
        {
            folderPath = ProcessFolderPath(folderPath);
        }
        
        using var client = CreateS3Client();
        
        var key = string.IsNullOrEmpty(folderPath) ? fileName : $"{folderPath}{fileName}";
        
        try
        {
            var fileTransferUtility = new TransferUtility(client);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = key,
                BucketName = bucketName,
                CannedACL = accessLevel
            };
            
            await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);
            
            var timestamp = DateTime.Now.Ticks;
            var rootPath = String.IsNullOrEmpty(cloudFrontPath) ? _getBucketUrl(bucketName) : cloudFrontPath;
            
            fileUrl = $"{rootPath}/{key}?v={timestamp}";
        }
        catch (Exception ex)
        {
            logger.LogError("[AwsStorageService.UploadToBucket]: {Message}. Inner exception: {InnerException}", ex.Message, ex.InnerException?.ToString());
        }
        
        return fileUrl;
    }

    private async Task<bool> DeleteFile(string bucketName, string key, CancellationToken cancellationToken = default)
    {
        var deleted = false;
        
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        using var client = CreateS3Client();
        
        try
        {
            var fileDeleteResponse = await client.DeleteObjectAsync(deleteRequest, cancellationToken);
            
            deleted = fileDeleteResponse.HttpStatusCode == System.Net.HttpStatusCode.OK ||
                fileDeleteResponse.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AwsStorageDervice.DeleteFileOrFolder]: {ex.Message}. Inner exception: {ex.InnerException}");
        }
        
        return deleted;
    }
}
