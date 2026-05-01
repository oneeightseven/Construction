using Construction.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioFileStorageService(IConfiguration config)
    {
        _minioClient = new MinioClient()
            .WithEndpoint(config["Minio:Endpoint"])
            .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
            .WithSSL(false)
            .Build();

        _bucketName = config["Minio:FilesBucket"] ?? "files";
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var objectName = $"{Guid.NewGuid()}_{fileName}";

        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putArgs, cancellationToken);

        return objectName;
    }

    public async Task<Stream> GetAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var memoryStream = new MemoryStream();

            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoryStream);
                    }),
                cancellationToken
            );

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch
        {
            return default;
        }
    }

    public async Task DeleteAsync(string objectName, CancellationToken cancellationToken = default)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName),
            cancellationToken
        );
    }
}