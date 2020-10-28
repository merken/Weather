using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weather.Contract
{
    public class WeatherForecast
    {
        /// <summary>
        /// Day of the week
        /// </summary>
        /// <value></value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Overall temperature in Celsius
        /// </summary>
        /// <value></value>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Short summary of the weather for that day
        /// </summary>
        /// <value></value>
        public string Summary { get; set; }
    }

    public interface IWeatherPlugin
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherFor(string city);
    }
}
