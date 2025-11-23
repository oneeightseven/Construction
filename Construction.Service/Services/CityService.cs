using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;
        public CityService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }
        public async Task<List<CityDto>> GetAllAsync()
        {
            List<CityDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<CityDto>>(CacheConstants.ALL_CITIES);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var cities = await _context.Cities.ToListAsync();
                    result = CityMapping.Map(cities);
                    await _minioCache.SetAsync(CacheConstants.ALL_CITIES, result);
                }

                _logger.LogInformation("Successful receipt of cities");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive cities {ex.Message}");
                return result;
            }
        }
    }
}
