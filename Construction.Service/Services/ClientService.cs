using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Construction.Models.Models;


namespace Construction.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            var result = await _context.Clients.ToListAsync();

            return ClientMapping.Map(result);
        }


        public async Task<string> UpdateAsync(ClientDto client)
        {
            if (client.Id == 0)
            {
                Client dbClient = new()
                {
                    Name = client.Name
                };

                await _context.Clients.AddAsync(dbClient);
            }
            else
            {
                var dbClient = await _context.Clients.FindAsync(client.Id);

                if (dbClient == null) return "404";

                dbClient.Name = client.Name;
            }

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null) return "404";

            _context.Clients.Remove(client);

            var result = await _context.SaveChangesAsync();

            return result == 1 ? "200" : "500";
        }
    }
}
