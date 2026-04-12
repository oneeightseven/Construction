using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;

        public StatusService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }
        public async Task<List<StatusDto>> GetAllAsync()
        {
            List<StatusDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<StatusDto>>(CacheConstants.ALL_STATUSES);
                if (cache != null) 
                {
                    result = cache;
                }
                else
                {
                    var statuses = await _context.Statuses.AsNoTracking().ToListAsync();
                    result = StatusMapping.Map(statuses);
                    await _minioCache.SetAsync(CacheConstants.ALL_STATUSES, result);
                }

                _logger.LogInformation("Successful receipt of statuses");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive statuses {ex.Message}");
                return result;
            }
        }
    }
}
