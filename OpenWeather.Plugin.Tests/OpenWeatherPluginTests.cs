using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenWeather.Plugin.Services;

namespace OpenWeather.Plugin.Tests
{
    [TestClass]
    public class OpenWeatherPluginTests
    {
        [TestMethod]
        public async Task GetWeatherFor_Works()
        {
            var city = "brussels,be";
            var now = DateTimeOffset.Now;
            var nowInUnix = now.ToUnixTimeSeconds();
            var description = "sunny";
            var temperatureInKelvin = 280.0m;
            var temperatureInC = (int)(temperatureInKelvin - 273.15m);

            // Create mock for the IOpenWeatherService
            var openWeatherServiceMock = new Mock<IOpenWeatherService>(MockBehavior.Strict);
            // Use the actual ConverterService 
            var converterService = new ConverterService();

            var responseModel = new Models.OpenWeatherResponseModel
            {
                list = new List<Models.OpenWeatherModel>()
                {
                    new Models.OpenWeatherModel
                    {
                        dt = now.ToUnixTimeSeconds(),
                        weather = new List<Models.OpenWeatherForecast>()
                        {
                            new Models.OpenWeatherForecast
                            {
                                description  = description
                            }
                        },
                        main = new Models.OpenWeatherTemperature
                        {
                                temp = temperatureInKelvin 
                        }
                    }
                }
            };

            openWeatherServiceMock.Setup(w => w.GetForecastsFor(city)).ReturnsAsync(responseModel);

            // Instantiate the Plugin, Prise.Testing will inject the Fields with the openWeatherService 
            var plugin = Prise.Testing.CreateTestPluginInstance<OpenWeatherPlugin>(openWeatherServiceMock.Object, converterService);

            var results = await plugin.GetWeatherFor(city);
            Assert.AreEqual(description, results.First().Summary);
            Assert.AreEqual(temperatureInC, results.First().TemperatureC);
            Assert.AreEqual(now.DayOfWeek, results.First().Date.DayOfWeek);
        }
    }
}
