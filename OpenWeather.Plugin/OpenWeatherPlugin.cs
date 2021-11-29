using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Prise.Plugin;
using Weather.Contract;
using OpenWeather.Plugin.Services;

namespace OpenWeather.Plugin;

// This makes the Plugin discoverable for Prise
[Plugin(PluginType = typeof(IWeatherPlugin))]
public class OpenWeatherPlugin : IWeatherPlugin
{
    [PluginService(ServiceType = typeof(IOpenWeatherService))]
    private readonly IOpenWeatherService openWeatherService;

    [PluginService(ServiceType = typeof(IConverterService))]
    private readonly IConverterService converter;

    [PluginActivated]
    public void Activated()
    {
        // We can access any PluginService here!
        // Or do some logging
        Console.WriteLine("I was activated!");
    }

    // The Contract method we need to implement
    public async Task<IEnumerable<WeatherForecast>> GetWeatherFor(string location)
    {
        var openWeatherResponseModel = await this.openWeatherService.GetForecastsFor(location);

        return this.converter.ConvertToWeatherForecasts(openWeatherResponseModel);
    }
}