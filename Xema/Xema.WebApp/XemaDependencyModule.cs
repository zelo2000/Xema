using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xema.ClusterAPI;
using Xema.Core.Models.Configuration;
using Xema.Services;
using Xema.Services.Infrastructure;
using Xema.Services.Provider;

namespace Xema.WebApp
{
    public static class XemaDependencyModule
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddClusterApi(configuration)
                .AddTransient<ICrossInhibitionService, CrossInhibitonSevice>()
                .AddTransient<IClusterApiClient, ClusterApiClient>()
                .AddTransient<IExcelService, ExcelService>()
                .AddTransient<IStyleProvider, StyleProvider>();

            return services;
        }

        private static IServiceCollection AddClusterApi(this IServiceCollection services, IConfiguration configuration)
        {
            var clusterApiSection = configuration.GetSection("ClusterApi");
            var clusterApiSettings = clusterApiSection.Get<ClusterApiSettings>();

            services.AddSingleton(clusterApiSettings)
                .AddHttpClient(clusterApiSettings.Name, c =>
                {
                    c.BaseAddress = new Uri(clusterApiSettings.BaseUrl);
                });

            return services;
        }
    }
}
