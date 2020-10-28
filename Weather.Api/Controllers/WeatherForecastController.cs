using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prise;
using Weather.Contract;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        // Inject the Prise Default IPluginLoader
        private readonly IPluginLoader weatherPluginLoader;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPluginLoader weatherPluginLoader)
        {
            _logger = logger;
            this.weatherPluginLoader = weatherPluginLoader;
        }

        // Add the location parameter to the route
        [HttpGet("{location}")]
        public async Task<IEnumerable<WeatherForecast>> Get(string location)
        {
            // pathToBinDebug = Weather.Api/bin/Debug/netcoreapp3.1
            var pathToBinDebug = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // pathToDist = _dist
            var pathToDist = Path.GetFullPath("../../../../_dist", pathToBinDebug);
            // scanResult should contain the information about the OpenWeather.Plugin
            var scanResult = await this.weatherPluginLoader.FindPlugin<IWeatherPlugin>(pathToDist);

            if (scanResult == null)
            {
                _logger.LogWarning($"No plugin was found for type {typeof(IWeatherPlugin).Name}");
                return null;
            }

            // Load the IWeatherPlugin
            var plugin = await this.weatherPluginLoader.LoadPlugin<IWeatherPlugin>(scanResult);

            // Invoke the IWeatherPlugin
            return await plugin.GetWeatherFor(location);
        }
    }
}
