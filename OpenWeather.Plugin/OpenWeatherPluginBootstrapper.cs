using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenWeather.Plugin.Services;
using Prise.Plugin;

namespace OpenWeather.Plugin
{
    // This bootstrapper is linked to our OpenWeatherPlugin class
    [PluginBootstrapper(PluginType = typeof(OpenWeatherPlugin))]
    public class OpenWeatherPluginBootstrapper : IPluginBootstrapper
    {
        // A fresh IServiceCollection is provided upon activation of the OpenWeatherPlugin
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Add the HttpClient
            services.AddScoped<HttpClient>(sp =>
            {
                var client = new HttpClient();
                // TODO get the endpoint from configuration
                var endpoint = "https://api.openweathermap.org/data/2.5/";
                client.BaseAddress = new Uri(endpoint);
                return client;
            });

            // Add the domain services using an interface registration
            services.AddScoped<IOpenWeatherService, OpenWeatherService>();
            services.AddScoped<IConverterService, ConverterService>();

            return services;
        }
    }
}