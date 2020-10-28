using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Prise.Plugin;
using Weather.Contract;

namespace OpenWeather.Plugin
{
    // This makes the Plugin discoverable for Prise
    [Plugin(PluginType = typeof(IWeatherPlugin))]
    public class OpenWeatherPlugin : IWeatherPlugin
    {
        // The Contract method we need to implement
        public async Task<IEnumerable<WeatherForecast>> GetWeatherFor(string location)
        {
            var openWeatherApi = "https://api.openweathermap.org/data/2.5/";
            var apiKey = "fd1f28913df6e270e48ea04536e3daba";
            var forecastEndpoint = $"forecast?q={location}&appid={apiKey}";

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(openWeatherApi);

            var response = await httpClient.GetAsync(forecastEndpoint);
            if (!response.IsSuccessStatusCode)
                throw new Exception("API did not respond with success.");

            var content = await response.Content.ReadAsStringAsync();
            var openWeatherModel = JsonSerializer.Deserialize<OpenWeatherResponseModel>(content);
            var results = openWeatherModel.list.Select(m => MapToWeatherForecast(m));
            var resultsPerDay = results.GroupBy(r => r.Date.DayOfWeek).Select(g => g.First());

            return resultsPerDay;
        }

        private WeatherForecast MapToWeatherForecast(OpenWeatherModel model)
        {
            return new WeatherForecast
            {
                Date = FromUnix(model.dt),
                Summary = model.weather.ElementAt(0).description,
                TemperatureC = FromKelvinToCelsius(model.main.temp)
            };
        }

        private DateTime FromUnix(long unixTimeStamp)
            => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();

        private int FromKelvinToCelsius(decimal kelvin)
            => (int)(kelvin - 273.15m);
    }

    class OpenWeatherForecast
    {
        public string main { get; set; }
        public string description { get; set; }
    }

    class OpenWeatherTemperature
    {
        // Temperature in Kelvin
        public decimal temp { get; set; }
    }

    class OpenWeatherModel
    {
        // Day of the week
        public long dt { get; set; }
        public OpenWeatherTemperature main { get; set; }
        public List<OpenWeatherForecast> weather { get; set; }
    }

    class OpenWeatherResponseModel
    {
        public List<OpenWeatherModel> list { get; set; }
    }
}