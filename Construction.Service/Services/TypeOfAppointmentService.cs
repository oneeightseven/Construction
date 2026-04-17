using Construction.Models.Dtos;
using Construction.Service.Contexts;
using Construction.Service.Extensions;
using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Service.Services
{
    public class TypeOfAppointmentService : ITypeOfAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMinioCacheService _minioCache;
        private readonly ILogger<TypeOfAppointmentService> _logger;
        public TypeOfAppointmentService(ApplicationDbContext context, IMinioCacheService minioCache, ILogger<TypeOfAppointmentService> logger)
        {
            _context = context;
            _minioCache = minioCache;
            _logger = logger;
        }
        public async Task<List<TypeOfAppointmentDto>> GetAllAsync()
        {
            List<TypeOfAppointmentDto> result = new();
            try
            {
                var cache = await _minioCache.GetAsync<List<TypeOfAppointmentDto>>(CacheConstants.ALL_TYPE_OF_APPOINTMENT);
                if (cache != null)
                {
                    result = cache;
                }
                else
                {
                    var objs = await _context.TypesOfAppointments.AsNoTracking().ToListAsync();
                    result = TypeOfAppointmentMapping.Map(objs);
                    await _minioCache.SetAsync(CacheConstants.ALL_TYPE_OF_APPOINTMENT, result);
                }

                _logger.LogInformation(LogHelper.SuccessGet("typeOfAppointment"));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(LogHelper.BadGet("typeOfAppointment"), ex.Message);
                return result;
            }
        }
    }
}
