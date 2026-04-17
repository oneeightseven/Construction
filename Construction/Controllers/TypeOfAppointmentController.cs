using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TypeOfAppointmentController : ControllerBase
    {
        private readonly ITypeOfAppointmentService _typeOfAppointmentService;
        public TypeOfAppointmentController(ITypeOfAppointmentService typeOfAppointmentService)
        {
            _typeOfAppointmentService = typeOfAppointmentService;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var objs = await _typeOfAppointmentService.GetAllAsync();
            return Ok(objs);
        }
    }
}
