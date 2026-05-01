using Construction.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace Construction.Service.Interfaces
{
    public interface IAppendicesService
    {
        public Task<Guid> UploadAsync(IFormFile file, string newName, DateTime newDate, int workId, CancellationToken cancellationToken);
        public Task<List<AppendicesByWorkIdDto>> GetAppendicesByWorkId(int workId, CancellationToken cancellationToken);
        public Task<int> RemoveById(Guid id, CancellationToken cancellationToken);
        public Task<Models.Dtos.FileStream> DownloadFile(Guid id, CancellationToken cancellationToken);
    }
}
