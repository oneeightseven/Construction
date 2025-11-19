using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class WorkService : IWorkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;

        public WorkService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }
        
        public async Task<List<WorkDto>> GetAllAsync()
        {
            string cacheKey = "allWorks";
            List<WorkDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<WorkDto>>(cacheKey);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var works = await _context.Works.Include(x => x.Brand)
                                                     .Include(x => x.City)
                                                     .Include(x => x.ShoppingMall)
                                                     .Include(x => x.Client)
                                                     .Include(x => x.Status)
                                                     .Include(x => x.ConstructionObject)
                                                     .Include(x => x.Employee)
                                                     .ToListAsync();

                    result = WorkMapping.Map(works);
                    await _minioCache.SetAsync(cacheKey, result, 1);
                }

                _logger.LogInformation("Successful receipt of works");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive works {ex.Message}");
                return result;
            }
            
        }

        public async Task<List<WorkDto>> GetByDateRange(DateOnly dateFrom, DateOnly dateTo)
        {
            var result = await GetAllAsync();

            var filteredResult = result.Where(x => x.Status!.Name == "Закрыт" &&
                                                   x.CompletionDate >= dateFrom &&
                                                   x.CompletionDate <= dateTo)
                                       .ToList();

            return filteredResult;
        }

        public async Task<string> UpdateWork(WorkDto model)
        {
            if (model?.Id == null || model.Id == 0) return "400";

            var existingWork = await _context.Works.FindAsync(model.Id);
            if (existingWork == null) return "404";

            existingWork.BrandId = model.Brand!.Id;
            existingWork.CityId = model.City!.Id;
            existingWork.ClientId = model.Client!.Id;
            existingWork.CompletionDate = model.CompletionDate;
            existingWork.ConstructionObjectId = model.ConstructionObject!.Id;
            existingWork.DateBid = model.DateBid;
            existingWork.DateOfCreation = model.DateOfCreation;
            existingWork.EmployeeId = model.Employee!.Id;
            existingWork.ShoppingMallId = model.ShoppingMall!.Id;
            existingWork.StatusId = model.Status!.Id;
            existingWork.Summ = model.Summ;
            existingWork.Term = model.Term;

            var result = await _context.SaveChangesAsync();
            return result == 1 ? "200" : "500";
        }
    }
}
 