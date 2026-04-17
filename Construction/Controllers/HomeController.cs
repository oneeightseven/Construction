using Construction.Models;
using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Construction.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController(){}

        [HttpPost(nameof(Ping))]
        public IActionResult Ping() => Ok("Pong");
    }
}
