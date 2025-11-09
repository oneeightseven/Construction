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
            var clients = await _context.Clients.ToListAsync();
            var result = ClientMapping.Map(clients);
            return result;
        }

    public async Task<string> UpdateAsync(ClientDto client)
        {
            if (client.Id == 0)
            {
                Client dbclient = new()
                {
                    Name = client.Name
                };

                await _context.Clients.AddAsync(dbclient);
            }
            else
            {
                var dbclient = await _context.Clients.FindAsync(client.Id);

                if (dbclient == null) return "404";

                dbclient.Name = client.Name;
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
