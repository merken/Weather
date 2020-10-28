using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenWeather.Plugin.Models;
using Weather.Contract;

namespace OpenWeather.Plugin.Services
{
    public interface IOpenWeatherService
    {
        Task<OpenWeatherResponseModel> GetForecastsFor(string location);
    }

    internal class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient client;
        private readonly string forecastApiFormat;
        public OpenWeatherService(
            HttpClient client, 
            IConfigurationService configurationService) // Injected via standard DI
        {
            this.client = client;
            // Get the API key from Appsettings.json
            var apiKey = configurationService.GetConfigurationValueForKey("OpenWeather:Key");
            this.forecastApiFormat = "forecast?q={0}&appid=" + apiKey;
        }

        public async Task<OpenWeatherResponseModel> GetForecastsFor(string location)
        {
            var response = await this.client.GetAsync(String.Format(this.forecastApiFormat, location));
            if (!response.IsSuccessStatusCode)
                throw new Exception("API did not respond with success.");

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OpenWeatherResponseModel>(content);
        }
    }
}