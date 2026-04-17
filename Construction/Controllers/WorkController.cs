using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IWorkService _workService;
        public WorkController(IWorkService workService)
        {
            _workService = workService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var works = await _workService.GetAllAsync();
            return Ok(works);
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] WorkDto model)
        {
            var result = await _workService.UpdateWork(model);
            return Ok(result);
        }
    }
}
