using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenWeather.Plugin.Services;
using Prise.Plugin;
using Weather.Contract;

namespace OpenWeather.Plugin
{
    [PluginBootstrapper(PluginType = typeof(OpenWeatherPlugin))]
    public class OpenWeatherPluginBootstrapper : IPluginBootstrapper
    {
        // Add a BootstrapperService
        [BootstrapperService(
            ServiceType = typeof(IConfigurationService), // The Weather.Contract.IConfigurationService interface
            ProxyType = typeof(ConfigurationServiceProxy))] // The ReverseProxy type that lives inside of this project
        private readonly IConfigurationService configurationService;

        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            services.AddScoped<HttpClient>(sp =>
            {
                var client = new HttpClient();
                // Use the IConfigurationService here
                var endpoint = this.configurationService.GetConfigurationValueForKey("OpenWeather:Endpoint");
                client.BaseAddress = new Uri(endpoint);
                return client;
            });

            services.AddScoped<IConfigurationService>(sp => this.configurationService);
            services.AddScoped<IOpenWeatherService, OpenWeatherService>();
            services.AddScoped<IConverterService, ConverterService>();

            return services;
        }
    }
}