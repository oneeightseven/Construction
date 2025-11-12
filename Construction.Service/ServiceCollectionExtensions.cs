using Construction.Service.Helpers;
using Construction.Service.Interfaces;
using Construction.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Construction.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConstructionServices(this IServiceCollection services)
        {
            //DB containers
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IShoppingMallService, ShoppingMallService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IConstructionObjectService, ConstructionObjectService>();

            //Another containers
            services.AddScoped<IExcelHelper, ExcelHelper>();

            return services;
        }
    }
}
