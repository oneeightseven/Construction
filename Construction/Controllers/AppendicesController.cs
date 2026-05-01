using Construction.Models;
using Construction.Service.Interfaces;
using Construction.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppendicesController : ControllerBase
    {
        private readonly IAppendicesService _appendicesService;
        public AppendicesController(IAppendicesService appendicesService)
        {
            _appendicesService = appendicesService;
        }

        [HttpPost(nameof(Upload))]
        public async Task<IActionResult> Upload([FromForm] UploadAppendixRequest request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length == 0) return BadRequest("Файл не выбран");

            var id = await _appendicesService.UploadAsync(request.File, request.Name, request.Date, request.WorkId, cancellationToken);

            return Ok(new { id });
        }

        [HttpPost(nameof(GetByWorkId))]
        public async Task<IActionResult> GetByWorkId([FromBody] int id, CancellationToken cancellationToken)
        {
            var result = await _appendicesService.GetAppendicesByWorkId(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(RemoveById))]
        public async Task<IActionResult> RemoveById([FromBody] Guid id, CancellationToken cancellationToken)
        {
            var result = await _appendicesService.RemoveById(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(DownloadFile))]
        public async Task<IActionResult> DownloadFile([FromBody] Guid id, CancellationToken cancellationToken)
        {
            var file = await _appendicesService.DownloadFile(id, cancellationToken);

            if (file == null)
                return NotFound();

            return File(
                file.Stream,
                file.ContentType,
                file.FileName
            );
        }
    }
}
