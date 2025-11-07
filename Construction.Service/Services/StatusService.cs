using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;

        public StatusService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<StatusDto>> GetAllAsync()
        {
            var statuses = await _context.Statuses.AsNoTracking().ToListAsync();

            var result = StatusMapping.Map(statuses);

            return result;
        }
    }
}
