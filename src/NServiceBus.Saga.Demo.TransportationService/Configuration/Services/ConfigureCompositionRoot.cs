﻿namespace NServiceBus.Saga.Demo.TransportationService.Configuration.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigureCompositionRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();
            return services;
        }
    }
}