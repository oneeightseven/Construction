using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialSerivce _materialService;
        public MaterialController(IMaterialSerivce materialSerivce)
        {
            _materialService = materialSerivce;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _materialService.GetAllAsync();
            return Ok(result);
        }
    }
}
