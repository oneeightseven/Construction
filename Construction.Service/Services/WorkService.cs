using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class WorkService : IWorkService
    {
        private readonly ApplicationDbContext _context;

        public WorkService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<WorkDto>> GetAllAsync()
        {
            var result = await _context.Works.Include(x => x.Brand)
                                             .Include(x => x.City)
                                             .Include(x => x.ShoppingMall)
                                             .Include(x => x.Client)
                                             .Include(x => x.Status)
                                             .Include(x => x.ConstructionObject)
                                             .Include(x => x.Employee)
                                             .ToListAsync();

            return WorkMapping.Map(result);
        }
    }
}
 