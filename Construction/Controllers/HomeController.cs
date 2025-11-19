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
        private readonly IJobTitleService _jobTitleService;
        private readonly IWorkService _workService;
        private readonly IStatusService _statusService;
        private readonly IClientService _clientService;
        private readonly IShoppingMallService _shoppingMallService;
        private readonly ICityService _cityService;
        private readonly IConstructionObjectService _constructionObjectService;
        private readonly IExcelHelper _excelHelper;
        public HomeController(IJobTitleService jobTitleService,
                              IWorkService workService,
                              IStatusService statusService,
                              IClientService clientService,
                              IShoppingMallService shoppingMallService,
                              ICityService cityService,
                              IConstructionObjectService constructionObjectService,
                              IExcelHelper excelHelper)
        {
            _jobTitleService = jobTitleService;
            _workService = workService;
            _statusService = statusService;
            _clientService = clientService;
            _shoppingMallService = shoppingMallService;
            _cityService = cityService;
            _constructionObjectService = constructionObjectService;
            _excelHelper = excelHelper;
        }

        [HttpGet("GetTitles")]
        public async Task<IActionResult> GetTitles()
        {
            var jobTitles = await _jobTitleService.GetAllJobTitlesAsync();
            return Ok(jobTitles);
        }

        [HttpGet("GetWorks")]
        public async Task<IActionResult> GetWorks()
        {
            var works = await _workService.GetAllAsync();
            var firstWork = works.First();

            List<WorkDto> moq = new();
            for (int index = 1; index < 300; index++)
            {
                moq.Add(new WorkDto
                {
                    Id = index,
                    DateBid = firstWork.DateBid,
                    Term = firstWork.Term,
                    CompletionDate = firstWork.CompletionDate,
                    City = firstWork.City,
                    ShoppingMall = firstWork.ShoppingMall,
                    Brand = firstWork.Brand,
                    Status = firstWork.Status,
                    ConstructionObject = firstWork.ConstructionObject,
                    Client = firstWork.Client,
                    DateOfCreation = firstWork.DateOfCreation,
                    Summ = firstWork.Summ,
                    Employee = firstWork.Employee
                });
            }

            return Ok(moq);
        }

        [HttpGet("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _statusService.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpPost("UpdateClient")]
        public async Task<IActionResult> UpdateClient([FromBody] ClientDto model)
        {
            var result = await _clientService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteClient")]
        public async Task<IActionResult> DeleteClient([FromBody] int id)
        {
            var result = await _clientService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("GetShoppingMalls")]
        public async Task<IActionResult> GetShoppingMalls()
        {
            var shoppingMalls = await _shoppingMallService.GetAllAsync();
            return Ok(shoppingMalls);
        }

        [HttpPost("UpdateShoppingMall")]
        public async Task<IActionResult> UpdateShoppingMalls([FromBody] ShoppingMallDto model)
        {
            var result = await _shoppingMallService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteShoppingMall")]
        public async Task<IActionResult> DeleteShoppingMalls([FromBody] int id)
        {
            var result = await _shoppingMallService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(cities);
        }

        [HttpPost("UpdateWork")]
        public async Task<IActionResult> UpdateWork([FromBody] WorkDto model)
        {
            var result = await _workService.UpdateWork(model);
            return Ok(result);
        }

        [HttpGet("GetConstructionObjects")]
        public async Task<IActionResult> GetConstructionObjects()
        {
            var result = await _constructionObjectService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("UpdateConstructionObject")]
        public async Task<IActionResult> UpdateConstructionObject([FromBody] ConstructionObjectDto model)
        {
            var result = await _constructionObjectService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteConstructionObject")]
        public async Task<IActionResult> DeleteConstructionObject([FromBody] int id)
        {
            var result = await _constructionObjectService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("DownloadExcel")]
        public async Task<IActionResult> DownloadExcel([FromBody] DownloadExcelRequest request)
        {
            var works = await _workService.GetByDateRange(request.DateFrom, request.DateTo);
            var stream = _excelHelper.GenerateDetailing(works);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "objects.xlsx");
        }
    }
}
