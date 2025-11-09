using Construction.Models.Dtos;
using Construction.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;
        private readonly IWorkService _workService;
        private readonly IStatusService _statusSerivce;
        private readonly IClientService _clientService;
        private readonly IShoppingMallService _shoppingMallService;
        private readonly ICityService _cityService;
        private readonly IConstructionObjectSerivce _constructionObjectSerivce;
        public HomeController(IJobTitleService jobTitleService, 
                              IWorkService workService, 
                              IStatusService statusSerivce, 
                              IClientService clientService, 
                              IShoppingMallService shoppingMallService,
                              ICityService cityService,
                              IConstructionObjectSerivce constructionObjectSerivce)
        {
            _jobTitleService = jobTitleService;
            _workService = workService;
            _statusSerivce = statusSerivce;
            _clientService = clientService;
            _shoppingMallService = shoppingMallService;
            _cityService = cityService;
            _constructionObjectSerivce = constructionObjectSerivce;
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
            var statuses = await _statusSerivce.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet("GetClients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientService.GetAll();
            return Ok(clients);
        }

        [HttpGet("GetShoppingMalls")]
        public async Task<IActionResult> GetShoppingMalls()
        {
            var shoppingMalls = await _shoppingMallService.GetAllAsync();
            return Ok(shoppingMalls);
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
            var result = await _constructionObjectSerivce.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("UpdateConstructionObject")]
        public async Task<IActionResult> UpdateConstructionObject([FromBody] ConstructionObjectDto model)
        {
            var result = await _constructionObjectSerivce.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteConstructionObject")]
        public async Task<IActionResult> DeleteConstructionObject([FromBody] int id)
        {
            var result = await _constructionObjectSerivce.DeleteAsync(id);
            return Ok(result);
        }
    }
}
