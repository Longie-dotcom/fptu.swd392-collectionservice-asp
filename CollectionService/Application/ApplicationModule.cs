using Application.Helper;
using Application.Interface.IService;
using Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationModule
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Methods
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

            services.AddScoped<ICollectorService, CollectorService>();

            return services;
        }
        #endregion
    }
}
