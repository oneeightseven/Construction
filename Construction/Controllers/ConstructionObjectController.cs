using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConstructionObjectController : ControllerBase
    {
        private readonly IConstructionObjectService _constructionObjectService;

        public ConstructionObjectController(IConstructionObjectService constructionObjectService)
        {
            _constructionObjectService = constructionObjectService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _constructionObjectService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] ConstructionObjectDto model)
        {
            var result = await _constructionObjectService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await _constructionObjectService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
