using System.Collections.Generic;

namespace OpenWeather.Plugin.Models
{
    public class OpenWeatherModel
    {
        // Day of the week
        public long dt { get; set; }
        public OpenWeatherTemperature main { get; set; }
        public List<OpenWeatherForecast> weather { get; set; }
    }
}
