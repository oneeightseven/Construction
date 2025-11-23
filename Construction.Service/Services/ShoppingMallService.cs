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
    public class ShoppingMallService : IShoppingMallService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;

        public ShoppingMallService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<List<ShoppingMallDto>> GetAllAsync()
        {
            List<ShoppingMallDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<ShoppingMallDto>>(CacheConstants.ALL_SHOPPING_MALLS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var shoppingMalls = await _context.ShoppingMalls.ToListAsync();
                    result = ShoppingMallMapping.Map(shoppingMalls);
                    await _minioCache.SetAsync(CacheConstants.ALL_SHOPPING_MALLS, result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("shoppingMalls"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("shoppingMalls"), ex.Message);
                return result;
            }
        }

        public async Task<string> UpdateAsync(ShoppingMallDto obj)
        {
            try
            {
                if (obj.Id == 0)
                {
                    ShoppingMall shoppingMall = new()
                    {
                        Name = obj.Name,
                        Address = obj.Address,
                        Contact = obj.Contact
                    };

                    await _context.ShoppingMalls.AddAsync(shoppingMall);
                }
                else
                {
                    var dbObject = await _context.ShoppingMalls.FindAsync(obj.Id);

                    if (dbObject == null)
                    {
                        _logger.LogError(LogHelper.NotFoundMessage("ShoppingMall", obj.Id));
                        return NotFound;
                    }

                    dbObject.Name = obj.Name;
                    dbObject.Address = obj.Address;
                    dbObject.Contact = obj.Contact;
                }

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_SHOPPING_MALLS);

                    _logger.LogInformation(LogHelper.SuccessUpdate("ShoppingMall", obj.Id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("ShoppingMall", obj.Id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("ShoppingMall", obj.Id), ex.Message);
                return BadRequest;
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var obj = await _context.ShoppingMalls.FindAsync(id);

                if (obj == null)
                {
                    _logger.LogError(LogHelper.NotFoundMessage("ShoppingMall", id));
                    return NotFound;
                }

                _context.ShoppingMalls.Remove(obj);

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_SHOPPING_MALLS);

                    _logger.LogInformation(LogHelper.SuccessRemove("ShoppingMall", id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("ShoppingMall", id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("ShoppingMall", id), ex.Message);
                return BadRequest;
            }
            
        }
    }
}
