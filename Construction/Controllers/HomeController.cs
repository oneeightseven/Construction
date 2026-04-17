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
        private readonly IWorkSmetaService _workSmetaSerivce;
        private readonly IMaterialSerivce _materialService;
        private readonly IAccountSerivce _accountSerivce;
        public HomeController(IJobTitleService jobTitleService,
                              IWorkService workService,
                              IStatusService statusService,
                              IClientService clientService,
                              IShoppingMallService shoppingMallService,
                              ICityService cityService,
                              IConstructionObjectService constructionObjectService,
                              IExcelHelper excelHelper,
                              IWorkSmetaService workSmetaSerivce,
                              IMaterialSerivce materialSerivce,
                              IAccountSerivce accountSerivce)
        {
            _jobTitleService = jobTitleService;
            _workService = workService;
            _statusService = statusService;
            _clientService = clientService;
            _shoppingMallService = shoppingMallService;
            _cityService = cityService;
            _constructionObjectService = constructionObjectService;
            _excelHelper = excelHelper;
            _workSmetaSerivce = workSmetaSerivce;
            _materialService = materialSerivce;
            _accountSerivce = accountSerivce;
        }

        [HttpGet(nameof(GetTitles))]
        public async Task<IActionResult> GetTitles()
        {
            var jobTitles = await _jobTitleService.GetAllJobTitlesAsync();
            return Ok(jobTitles);
        }

        [HttpGet(nameof(GetWorks))]
        public async Task<IActionResult> GetWorks()
        {
            var works = await _workService.GetAllAsync();
            return Ok(works);
        }

        [HttpGet(nameof(GetStatuses))]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _statusService.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet(nameof(GetClients))]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpPost(nameof(UpdateClient))]
        public async Task<IActionResult> UpdateClient([FromBody] ClientDto model)
        {
            var result = await _clientService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(DeleteClient))]
        public async Task<IActionResult> DeleteClient([FromBody] int id)
        {
            var result = await _clientService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetShoppingMalls))]
        public async Task<IActionResult> GetShoppingMalls()
        {
            var shoppingMalls = await _shoppingMallService.GetAllAsync();
            return Ok(shoppingMalls);
        }

        [HttpPost(nameof(UpdateShoppingMalls))]
        public async Task<IActionResult> UpdateShoppingMalls([FromBody] ShoppingMallDto model)
        {
            var result = await _shoppingMallService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(DeleteShoppingMalls))]
        public async Task<IActionResult> DeleteShoppingMalls([FromBody] int id)
        {
            var result = await _shoppingMallService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetCities))]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(cities);
        }

        [HttpPost(nameof(UpdateWork))]
        public async Task<IActionResult> UpdateWork([FromBody] WorkDto model)
        {
            var result = await _workService.UpdateWork(model);
            return Ok(result);
        }

        [HttpGet(nameof(GetConstructionObjects))]
        public async Task<IActionResult> GetConstructionObjects()
        {
            var result = await _constructionObjectService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost(nameof(UpdateConstructionObject))]
        public async Task<IActionResult> UpdateConstructionObject([FromBody] ConstructionObjectDto model)
        {
            var result = await _constructionObjectService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost(nameof(DeleteConstructionObject))]
        public async Task<IActionResult> DeleteConstructionObject([FromBody] int id)
        {
            var result = await _constructionObjectService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost(nameof(DownloadExcel))]
        public async Task<IActionResult> DownloadExcel([FromBody] DownloadExcelRequest request)
        {
            var works = await _workService.GetByDateRange(request.DateFrom, request.DateTo);
            var stream = _excelHelper.GenerateDetailing(works);

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "objects.xlsx");
        }

        [HttpPost(nameof(GetWorkSmetaById))]
        public async Task<IActionResult> GetWorkSmetaById([FromBody] int id)
        {
            var result = await _workSmetaSerivce.GetSmetaByWorkIdAsync(id);
            return Ok(result);
        }

        [HttpGet(nameof(GetAllMaterials))]
        public async Task<IActionResult> GetAllMaterials()
        {
            var result = await _materialService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost(nameof(AddSmetaToWork))]
        public async Task<IActionResult> AddSmetaToWork([FromBody] AddSmetaToWorkDto model)
        {
            var result = await _workSmetaSerivce.AddSmetaToWork(model);
            return Ok(result);
        }

        [HttpPost(nameof(RemoveSmetaById))]
        public async Task<IActionResult> RemoveSmetaById([FromBody] int id)
        {
            var result = await _workSmetaSerivce.RemoveSmetaById(id);
            return Ok(result);
        }

        [HttpPost(nameof(GetAccountsByWorkId))]
        public async Task<IActionResult> GetAccountsByWorkId([FromBody] int id)
        {
            var result = await _accountSerivce.GetByWorkIdAsync(id);
            return Ok(result);
        }
    }
}
