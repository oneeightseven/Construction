using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Construction.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSerivce _accountSerivce;
        public AccountController(IAccountSerivce accountSerivce)
        {
            _accountSerivce = accountSerivce;
        }

        [HttpPost(nameof(GetByWorkId))]
        public async Task<IActionResult> GetByWorkId([FromBody] int id)
        {
            var result = await _accountSerivce.GetByWorkIdAsync(id);
            return Ok(result);
        }

        [HttpPost(nameof(AddToWork))]
        public async Task<IActionResult> AddToWork([FromBody] AddAcountToWorkDto model)
        {
            var result = await _accountSerivce.AddAccountToWork(model);
            return Ok(result);
        }

        [HttpPost(nameof(RemoveById))]
        public async Task<IActionResult> RemoveById([FromBody] int id)
        {
            var result = await _accountSerivce.RemoveById(id);
            return Ok(result);
        }
    }
}
