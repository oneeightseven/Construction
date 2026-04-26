using Construction.Models;
using Construction.Service.Interfaces;
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

            var id = await _appendicesService.UploadAsync(request.File, request.Name, request.Date, cancellationToken);

            return Ok(new { id });
        }
    }
}
