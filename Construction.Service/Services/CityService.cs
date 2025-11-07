using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;
        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CityDto>> GetAllAsync()
        {
            var clients = await _context.Cities.ToListAsync();
            var result = CityMapping.Map(clients);
            return result;
        }
    }
}
