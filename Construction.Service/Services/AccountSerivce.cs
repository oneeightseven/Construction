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
                var cache = await _minioCache.GetAsync<List<AccountDto>>($"{CacheConstants.ACCOUNTS_BY_WORK_ID}_{workId}");
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
                    await _minioCache.SetAsync($"{CacheConstants.ACCOUNTS_BY_WORK_ID}_{workId}", result);
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

        public async Task<int> AddAccountToWork(AddAcountToWorkDto model)
        {
            try
            {
                var utcDate = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc);

                Account entity;

                if (model.Id == null)
                {
                    entity = new Account
                    {
                        Date = utcDate,
                        Details = model.Details,
                        WorkId = model.WorkId,
                        TypeOfAppointmentId = model.TypeOfAppointmentId,
                        PayerId = model.PayerId,
                        Sum = model.Sum,
                    };

                    await _context.Accounts.AddAsync(entity);
                }
                else
                {
                    entity = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id);

                    if (entity == null)
                        return -1;

                    entity.Date = utcDate;
                    entity.Details = model.Details;
                    entity.WorkId = model.WorkId;
                    entity.TypeOfAppointmentId = model.TypeOfAppointmentId;
                    entity.PayerId = model.PayerId;
                    entity.Sum = model.Sum;
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation(LogHelper.SuccessUpdate("account to work", entity.Id));

                    await _minioCache.RemoveAsync($"{CacheConstants.ACCOUNTS_BY_WORK_ID}_{model.WorkId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error while saving work account");
                return -1;
            }
        }

        public async Task<int> RemoveById(int accountId)
        {
            int result = -1;
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
                if (account != null)
                {
                    _context.Accounts.Remove(account);
                    result = await _context.SaveChangesAsync();
                }

                if (result > 0)
                {
                    _logger.LogInformation(LogHelper.SuccessRemove("account from work", accountId));

                    await _minioCache.RemoveAsync($"{CacheConstants.ACCOUNTS_BY_WORK_ID}_{account.WorkId}");
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
