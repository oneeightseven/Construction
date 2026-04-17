using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Construction.Service.Services
{
    public class AccountSerivce : IAccountSerivce
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<AccountSerivce> _logger;

        public AccountSerivce(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<AccountSerivce> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }

        public async Task<List<AccountDto>> GetByWorkIdAsync(int workId)
        {
            List<AccountDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<AccountDto>>(CacheConstants.ALL_ACCOUNTS);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var models = await _context.Accounts.Where(x => x.WorkId == workId)
                                                        .Include(x => x.Payer)
                                                        .Include(x => x.TypeOfAppointment)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                    result = AccountMapping.Map(models);
                    await _minioCache.SetAsync(CacheConstants.ALL_ACCOUNTS, result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("accounts"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("accounts"), ex);
                return result;
            }
        }
    }
}
