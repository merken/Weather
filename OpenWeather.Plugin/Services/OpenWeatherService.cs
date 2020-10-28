using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenWeather.Plugin.Models;
using Weather.Contract;

namespace OpenWeather.Plugin.Services
{
    internal interface IOpenWeatherService
    {
        Task<OpenWeatherResponseModel> GetForecastsFor(string location);
    }

    internal class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient client;
        private readonly string forecastApiFormat;
        
        public OpenWeatherService(HttpClient client)
        {
            this.client = client;
            var apiKey = "fd1f28913df6e270e48ea04536e3daba";
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