using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class JobTitleService : IJobTitleService
    {
        private readonly ApplicationDbContext _context;

        public JobTitleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<List<JobTitle>> GetAllJobTitlesAsync()
        {
            return _context.JobTitles.ToListAsync();
        }
    }
}
