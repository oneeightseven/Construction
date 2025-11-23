using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Construction.Service.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkService> _logger;

        public ClientService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            List<ClientDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<ClientDto>>(CacheConstants.ALL_CLIENTS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var clients = await _context.Clients.ToListAsync();
                    result = ClientMapping.Map(clients);
                    await _minioCache.SetAsync(CacheConstants.ALL_CLIENTS, result);
                }

                _logger.LogInformation("Successful receipt of clients");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to receive clients {ex.Message}");
                return result;
            }
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
