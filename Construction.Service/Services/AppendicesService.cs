using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class AppendicesService: IAppendicesService
{
    private readonly IFileStorageService _storage;
    private readonly ApplicationDbContext _db;

    public AppendicesService(IFileStorageService storage, ApplicationDbContext db)
    {
        _storage = storage;
        _db = db;
    }

    public async Task<Guid> UploadAsync(IFormFile file, string newName, DateTime newDate, int workId, CancellationToken cancellationToken)
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
            CreatedAt = DateTime.SpecifyKind(newDate, DateTimeKind.Utc),
            WorkId = workId
        };

        _db.StoredFiles.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<List<AppendicesByWorkIdDto>> GetAppendicesByWorkId(int workId, CancellationToken cancellationToken)
    {
        var storedFiles = await _db.StoredFiles.Where(x => x.WorkId == workId).ToListAsync();

        return AppendicesByWorkIdMapping.Map(storedFiles);
    }

    public async Task<int> RemoveById(Guid id, CancellationToken cancellationToken)
    {
        var file = await _db.StoredFiles.FirstOrDefaultAsync(x => x.Id == id);
        if (file != null) {
            _db.StoredFiles.Remove(file);
            var result = await _db.SaveChangesAsync();
            if (result > 0)
            {
                await _storage.DeleteAsync(file.StoredFileName, cancellationToken);
                return 1;
            }
            else return result;
        }

        return -1;
    }

    public async Task<Construction.Models.Dtos.FileStream> DownloadFile(Guid id, CancellationToken cancellationToken)
    {
        var file = await _db.StoredFiles.FirstOrDefaultAsync(x => x.Id == id);

        return new()
        {
            FileName = file.OriginalFileName,
            ContentType = file.ContentType,
            Stream = await _storage.GetAsync(file.StoredFileName, cancellationToken)
        };
    }
}