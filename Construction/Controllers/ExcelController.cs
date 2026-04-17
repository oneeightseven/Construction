using Construction.Models;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly IExcelHelper _excelHelper;
        public ExcelController(IWorkService workService, 
                               IExcelHelper excelHelper)
        {
            _workService = workService;
            _excelHelper = excelHelper;
        }

        [HttpPost(nameof(Download))]
        public async Task<IActionResult> Download([FromBody] DownloadExcelRequest request)
        {
            var works = await _workService.GetByDateRange(request.DateFrom, request.DateTo);
            var stream = _excelHelper.GenerateDetailing(works);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "objects.xlsx");
        }
    }
}
