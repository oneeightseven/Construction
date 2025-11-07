using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Construction.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        public ClientService(ApplicationDbContext context) 
        { 
            _context = context;
        }
        public async Task<List<ClientDto>> GetAll()
        {
            var clients = await _context.Clients.ToListAsync();
            var result = ClientMapping.Map(clients);
            return result;
        }
    }
}
