using Microsoft.AspNetCore.Http;

namespace Construction.Service.Interfaces
{
    public interface IAppendicesService
    {
        public Task<Guid> UploadAsync(IFormFile file, string newName, DateTime newDate, CancellationToken cancellationToken);
    }
}
