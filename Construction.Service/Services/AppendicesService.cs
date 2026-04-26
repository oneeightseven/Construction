using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Http;

public class AppendicesService: IAppendicesService
{
    private readonly IFileStorageService _storage;
    private readonly ApplicationDbContext _db;

    public AppendicesService(IFileStorageService storage, ApplicationDbContext db)
    {
        _storage = storage;
        _db = db;
    }

    public async Task<Guid> UploadAsync(IFormFile file, string newName, DateTime newDate, CancellationToken cancellationToken)
    {
        using var stream = file.OpenReadStream();

        var objectName = await _storage.UploadAsync(stream, newName, file.ContentType, cancellationToken);

        var entity = new StoredFile
        {
            Id = Guid.NewGuid(),
            OriginalFileName = newName,
            StoredFileName = objectName,
            ContentType = file.ContentType,
            Size = file.Length,
            CreatedAt = newDate
        };

        _db.StoredFiles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}