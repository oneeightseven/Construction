using Construction.Service.Interfaces;
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
    }
}
