using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class ConstructionObjectService : IConstructionObjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;

        public ConstructionObjectService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<List<ConstructionObjectDto>> GetAllAsync()
        {
            List<ConstructionObjectDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<ConstructionObjectDto>>(CacheConstants.ALL_CONSTRUCTION_OBJECTS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var objects = await _context.ConstructionObjects.ToListAsync();
                    result = ConstructionObjectMapping.Map(objects);
                    await _minioCache.SetAsync(CacheConstants.ALL_CONSTRUCTION_OBJECTS, result);
                }

                _logger.LogInformation("Successful receipt of constructionObjects");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive constructionObjects {ex.Message}");
                return result;
            }
        }

        public async Task<string> UpdateAsync(ConstructionObjectDto obj)
        {
            if (obj.Id == 0)
            {
                ConstructionObject dbObject = new()
                {
                    Name = obj.Name
                };

                await _context.ConstructionObjects.AddAsync(dbObject);
            }
            else
            {
                var dbObject = await _context.ConstructionObjects.FindAsync(obj.Id);

                if (dbObject == null) return "404";

                dbObject.Name = obj.Name;
            }

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var obj = await _context.ConstructionObjects.FindAsync(id);

            if (obj == null) return "404";

            _context.ConstructionObjects.Remove(obj);

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }
    }
}
