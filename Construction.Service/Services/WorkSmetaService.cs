using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class WorkSmetaService : IWorkSmetaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkSmetaService> _logger;

        public WorkSmetaService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkSmetaService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<int> AddSmetaToWork(AddSmetaToWorkDto model)
        {
            var result = -1;
            try
            {
                WorkSmeta workSmeta = new()
                {
                    Count = model.Count,
                    Price = model.Price,
                    MaterialId = model.MaterialId,
                    WorkId = model.WorkId,
                };

                await _context.WorkSmetas.AddAsync(workSmeta);
                result = await _context.SaveChangesAsync();

                if (result > 0) { 
                    _logger.LogInformation(LogHelper.SuccessUpdate("smeta to work", 0));
                    await _minioCache.RemoveAsync($"{CacheConstants.SMETA_BY_WORK_ID}_{model.WorkId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("work smeta by id"), ex);
                return result;
            }
        }

        public async Task<List<WorkSmetaDto>> GetSmetaByWorkIdAsync(int workId)
        {
            List<WorkSmetaDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<WorkSmetaDto>>($"{CacheConstants.SMETA_BY_WORK_ID}_{workId}");
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var smetas = await _context.WorkSmetas.Where(x => x.WorkId == workId)
                                                         .Include(x => x.Material)
                                                         .ToListAsync();

                    result = WorkSmetaMapping.Map(smetas);
                    await _minioCache.SetAsync($"{CacheConstants.SMETA_BY_WORK_ID}_{workId}", result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("work smeta by id"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("work smeta by id"), ex);
                return result;
            }
        }
    }
}
