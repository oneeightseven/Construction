using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

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
            var shoppingMalls = await _context.ShoppingMalls.ToListAsync();

            var result = ShoppingMallMapping.Map(shoppingMalls);

            return result;
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
