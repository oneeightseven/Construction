namespace Construction.Service.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);

        Task<Stream> GetAsync(string objectName, CancellationToken cancellationToken = default);

        Task DeleteAsync(string objectName, CancellationToken cancellationToken = default);
    }
}
