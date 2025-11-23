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

                _logger.LogInformation(LogHelper.SuccessGet("constructionObjects"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("constructionObjects"), ex.Message);
                return result;
            }
        }

        public async Task<string> UpdateAsync(ConstructionObjectDto obj)
        {
            try
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

                    if (dbObject == null)
                    {
                        _logger.LogError(LogHelper.NotFoundMessage("constructionObject", obj.Id));
                        return NotFound;
                    }

                    dbObject.Name = obj.Name;
                }

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_CONSTRUCTION_OBJECTS);

                    _logger.LogInformation(LogHelper.SuccessUpdate("constructionObject", obj.Id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("constructionObject", obj.Id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("constructionObject", obj.Id), ex.Message);
                return BadRequest;
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var obj = await _context.ConstructionObjects.FindAsync(id);

                if (obj == null)
                {
                    _logger.LogError(LogHelper.NotFoundMessage("constructionObject", id));
                    return NotFound;
                }

                _context.ConstructionObjects.Remove(obj);

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_CONSTRUCTION_OBJECTS);

                    _logger.LogInformation(LogHelper.SuccessRemove("constructionObject", id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("constructionObject", id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("constructionObject", id), ex.Message);
                return BadRequest;
            }
            
        }
    }
}
