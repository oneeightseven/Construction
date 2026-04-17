using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class MaterialService : IMaterialSerivce
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<MaterialService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }
        public async Task<List<MaterialDto>> GetAllAsync()
        {
            List<MaterialDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<MaterialDto>>(CacheConstants.ALL_MATERIALS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var materials = await _context.Materials.ToListAsync();

                    result = MaterialMapping.Map(materials);
                    await _minioCache.SetAsync(CacheConstants.ALL_MATERIALS, result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("all materials"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("all materials"), ex);
                return result;
            }
        }
    }
}
