using Construction.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System.Text.Json;

public class MinioCacheService : IMinioCacheService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly JsonSerializerOptions _jsonOptions;

    public MinioCacheService(string endpoint, string accessKey, string secretKey, string bucketName = "cache")
    {
        _minioClient = new MinioClient().WithEndpoint(endpoint).WithCredentials(accessKey, secretKey).WithSSL(false).Build();

        _bucketName = bucketName;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        _ = EnsureBucketExists();
    }

    public MinioCacheService(IConfiguration config) : this(config.GetSection("Minio:Endpoint").Value!, config.GetSection("Minio:AccessKey").Value!, config.GetSection("Minio:SecretKey").Value!, config.GetSection("Minio:Bucket").Value!){}

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            using var stream = new MemoryStream();

            var getArgs = new GetObjectArgs().WithBucket(_bucketName).WithObject(key).WithCallbackStream(s => s.CopyTo(stream));

            await _minioClient.GetObjectAsync(getArgs);

            var jsonString = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            var cacheItem = JsonSerializer.Deserialize<CacheItem<T>>(jsonString, _jsonOptions);

            if (cacheItem == null || cacheItem.IsExpired)
            {
                if (cacheItem != null)
                {
                    await RemoveAsync(key);
                }
                return default(T);
            }

            return cacheItem.Data;
        }
        catch (Exception ex) when (ex.Message.Contains("NoSuchKey"))
        {
            return default(T);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading cache {key}: {ex.Message}");
            return default(T);
        }
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan maxLifetime)
    {
        try
        {
            var cacheItem = new CacheItem<T>
            {
                Data = data,
                CachedAt = DateTime.UtcNow,
                MaxLifetime = maxLifetime
            };

            var jsonString = JsonSerializer.Serialize(cacheItem, _jsonOptions);
            var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

            using var stream = new MemoryStream(bytes);

            var putArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(key)
                .WithStreamData(stream)
                .WithObjectSize(bytes.Length)
                .WithContentType("application/json");

            await _minioClient.PutObjectAsync(putArgs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing cache {key}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            var statArgs = new StatObjectArgs().WithBucket(_bucketName).WithObject(key);

            await _minioClient.StatObjectAsync(statArgs);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            var removeArgs = new RemoveObjectArgs().WithBucket(_bucketName).WithObject(key);

            await _minioClient.RemoveObjectAsync(removeArgs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing cache {key}: {ex.Message}");
        }
    }

    private async Task EnsureBucketExists()
    {
        try
        {
            var exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));

            if (!exists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating bucket: {ex.Message}");
        }
    }

    private class CacheItem<T>
    {
        public T Data { get; set; }
        public DateTime CachedAt { get; set; }
        public TimeSpan MaxLifetime { get; set; }

        public bool IsExpired => DateTime.UtcNow > CachedAt + MaxLifetime;
    }
}

