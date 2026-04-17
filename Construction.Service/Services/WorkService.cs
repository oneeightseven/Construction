using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using static Construction.Service.Extensions.ResponseConstants;

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
            List<WorkDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<WorkDto>>(CacheConstants.ALL_WORKS);
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
                    await _minioCache.SetAsync(CacheConstants.ALL_WORKS, result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("works"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("works"), ex);
                return result;
            }
            
        }

        public async Task<List<WorkDto>> GetByDateRange(DateOnly dateFrom, DateOnly dateTo)
        {
            try
            {
                var result = await GetAllAsync();

                var filteredResult = result.Where(x => x.Status!.Name == "Закрыт" &&
                                                       x.CompletionDate >= dateFrom &&
                                                       x.CompletionDate <= dateTo)
                                           .ToList();

                return filteredResult;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Something went wrong", ex.Message);
                return default;
            }
            
        }

        public async Task<string> UpdateWork(WorkDto model)
        {
            try
            {
                if (model?.Id == null || model.Id == 0)
                {
                    _logger.LogError(LogHelper.NotFoundMessage("work", model.Id));
                    return NotFound;
                }

                var existingWork = await _context.Works.FindAsync(model.Id);
                if (existingWork == null)
                {
                    _logger.LogError(LogHelper.NotFoundMessage("work", model.Id));
                    return NotFound;
                }

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
                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_WORKS);

                    _logger.LogInformation(LogHelper.SuccessUpdate("work", model.Id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("work", model.Id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("work", model.Id), ex.Message);
                return BadRequest;
            }
            
        }
    }
}
 