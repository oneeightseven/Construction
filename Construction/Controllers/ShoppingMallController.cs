using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShoppingMallController : ControllerBase
    {
        private readonly IShoppingMallService _shoppingMallService;
        public ShoppingMallController(IShoppingMallService shoppingMallService)
        {
            _shoppingMallService = shoppingMallService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var shoppingMalls = await _shoppingMallService.GetAllAsync();
            return Ok(shoppingMalls);
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] ShoppingMallDto model)
        {
            var result = await _shoppingMallService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await _shoppingMallService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
