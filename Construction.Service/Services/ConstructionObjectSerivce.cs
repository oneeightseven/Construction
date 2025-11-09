using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class ConstructionObjectSerivce : IConstructionObjectSerivce
    {
        private readonly ApplicationDbContext _context;

        public ConstructionObjectSerivce(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConstructionObjectDto>> GetAllAsync()
        {
            var result = await _context.ConstructionObjects.ToListAsync();

            return ConstructionObjectMapping.Map(result);
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
