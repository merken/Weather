using System.Collections.Generic;

namespace OpenWeather.Plugin.Models;

public class OpenWeatherResponseModel
{
    public List<OpenWeatherModel> list { get; set; }
}