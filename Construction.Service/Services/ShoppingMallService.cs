using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;

namespace Construction.Service.Services
{
    public class ShoppingMallService : IShoppingMallService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingMallService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingMallDto>> GetAllAsync()
        {
            var shoppingMalls = _context.ShoppingMalls.ToList();

            var result = ShoppingMallMapping.Map(shoppingMalls);

            return result;
        }
    }
}
