using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Prise.Plugin;
using Weather.Contract;
using OpenWeather.Plugin.Services;

namespace OpenWeather.Plugin
{
    // This makes the Plugin discoverable for Prise
    [Plugin(PluginType = typeof(IWeatherPlugin))]
    public class OpenWeatherPlugin : IWeatherPlugin
    {
        [PluginService(ServiceType = typeof(IOpenWeatherService))]
        private readonly IOpenWeatherService openWeatherService;

        [PluginService(ServiceType = typeof(IConverterService))]
        private readonly IConverterService converter;

        public async Task<IEnumerable<WeatherForecast>> GetWeatherFor(string location)
        {
            var openWeatherResponseModel = await this.openWeatherService.GetForecastsFor(location);

            return this.converter.ConvertToWeatherForecasts(openWeatherResponseModel);
        }
    }
}
