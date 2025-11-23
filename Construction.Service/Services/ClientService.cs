using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using static Construction.Service.Extensions.ResponseConstants;

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

                _logger.LogInformation(LogHelper.SuccessGet("clients"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("clients"), ex.Message);
                return result;
            }
        }


        public async Task<string> UpdateAsync(ClientDto client)
        {
            try
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

                    if (dbClient == null)
                    {
                        _logger.LogError(LogHelper.NotFoundMessage("client", client.Id));
                        return NotFound;
                    }

                    dbClient.Name = client.Name;
                }

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_CLIENTS);

                    _logger.LogInformation(LogHelper.SuccessUpdate("client", client.Id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("client", client.Id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("client", client.Id), ex.Message);
                return BadRequest;
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var client = await _context.Clients.FindAsync(id);

                if (client == null)
                {
                    _logger.LogError(LogHelper.NotFoundMessage("client", id));
                    return NotFound;
                }

                _context.Clients.Remove(client);

                var result = await _context.SaveChangesAsync();

                if (result == 1)
                {
                    await _minioCache.RemoveAsync(CacheConstants.ALL_CLIENTS);

                    _logger.LogInformation(LogHelper.SuccessRemove("client", id));

                    return OK;
                }
                else
                {
                    _logger.LogError(LogHelper.BadRequest("client", id));
                    return BadRequest;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadRequest("client", id), ex.Message);
                return BadRequest;
            }
        }
    }
}
