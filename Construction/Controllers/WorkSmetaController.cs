using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkSmetaController : ControllerBase
    {
        private readonly IWorkSmetaService _workSmetaSerivce;
        public WorkSmetaController(IWorkSmetaService workSmetaSerivce)
        {
            _workSmetaSerivce = workSmetaSerivce;
        }

        [HttpPost(nameof(GetByWorkId))]
        public async Task<IActionResult> GetByWorkId([FromBody] int id)
        {
            var result = await _workSmetaSerivce.GetSmetaByWorkIdAsync(id);
            return Ok(result);
        }

        [HttpPost(nameof(AddToWork))]
        public async Task<IActionResult> AddToWork([FromBody] AddSmetaToWorkDto model)
        {
            var result = await _workSmetaSerivce.AddSmetaToWork(model);
            return Ok(result);
        }

        [HttpPost(nameof(RemoveById))]
        public async Task<IActionResult> RemoveById([FromBody] int id)
        {
            var result = await _workSmetaSerivce.RemoveSmetaById(id);
            return Ok(result);
        }
    }
}
