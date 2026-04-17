using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpPost(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] ClientDto model)
        {
            var result = await _clientService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await _clientService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
