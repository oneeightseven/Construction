using Construction.Models.Dtos;
using Construction.Models.Models;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class WorkSmetaService : IWorkSmetaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<WorkSmetaService> _logger;

        public WorkSmetaService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<WorkSmetaService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<int> AddSmetaToWork(AddSmetaToWorkDto model)
        {
            try
            {
                WorkSmeta entity;

                if (model.Id == null)
                {
                    entity = new WorkSmeta
                    {
                        Count = model.Count,
                        Price = model.Price,
                        MaterialId = model.MaterialId,
                        WorkId = model.WorkId,
                    };

                    await _context.WorkSmetas.AddAsync(entity);
                }
                else
                {
                    entity = await _context.WorkSmetas.FirstOrDefaultAsync(x => x.Id == model.Id);

                    if (entity == null) return -1;

                    entity.Count = model.Count;
                    entity.Price = model.Price;
                    entity.MaterialId = model.MaterialId;
                    entity.WorkId = model.WorkId;
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation(LogHelper.SuccessUpdate("smeta to work", entity.Id));

                    await _minioCache.RemoveAsync($"{CacheConstants.SMETA_BY_WORK_ID}_{model.WorkId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while saving work smeta");
                return -1;
            }
        }

        public async Task<List<WorkSmetaDto>> GetSmetaByWorkIdAsync(int workId)
        {
            List<WorkSmetaDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<WorkSmetaDto>>($"{CacheConstants.SMETA_BY_WORK_ID}_{workId}");
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var smetas = await _context.WorkSmetas.Where(x => x.WorkId == workId)
                                                         .Include(x => x.Material)
                                                         .ToListAsync();

                    result = WorkSmetaMapping.Map(smetas);
                    await _minioCache.SetAsync($"{CacheConstants.SMETA_BY_WORK_ID}_{workId}", result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("work smeta by id"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("work smeta by id"), ex);
                return result;
            }
        }

        public async Task<int> RemoveSmetaById(int smetaId)
        {
            int result = -1;
            try
            {
                var smeta = await _context.WorkSmetas.FirstOrDefaultAsync(x => x.Id == smetaId);
                if (smeta != null)
                {
                    _context.WorkSmetas.Remove(smeta);
                    result = await _context.SaveChangesAsync();
                }

                if (result > 0)
                {
                    _logger.LogInformation(LogHelper.SuccessRemove("smeta from work", smetaId));

                    await _minioCache.RemoveAsync($"{CacheConstants.SMETA_BY_WORK_ID}_{smeta?.WorkId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while remove work smeta");
                return -1;
            }
        }
    }
}
