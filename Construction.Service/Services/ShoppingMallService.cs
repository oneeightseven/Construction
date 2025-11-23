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
                var cache = await _minioCache.GetAsync<List<ShoppingMallDto>>(CacheConstants.ALL_CONSTRUCTION_OBJECTS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var shoppingMalls = await _context.ShoppingMalls.ToListAsync();
                    result = ShoppingMallMapping.Map(shoppingMalls);
                    await _minioCache.SetAsync(CacheConstants.ALL_CONSTRUCTION_OBJECTS, result);
                }

                _logger.LogInformation("Successful receipt of shoppingMalls");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive shoppingMalls {ex.Message}");
                return result;
            }
        }

        public async Task<string> UpdateAsync(ShoppingMallDto obj)
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

                if (dbObject == null) return "404";

                dbObject.Name = obj.Name;
                dbObject.Address = obj.Address;
                dbObject.Contact = obj.Contact;
            }

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var obj = await _context.ShoppingMalls.FindAsync(id);

            if (obj == null) return "404";

            _context.ShoppingMalls.Remove(obj);

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }
    }
}
