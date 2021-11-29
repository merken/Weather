using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenWeather.Plugin.Models;
using Weather.Contract;

namespace OpenWeather.Plugin.Services;

internal class WeatherForecastMapperProfile : Profile
{
    public WeatherForecastMapperProfile()
    {
        CreateMap<OpenWeatherModel, WeatherForecast>()
            .ForMember(w => w.Date, opt => opt.MapFrom(m => FromUnix(m.dt)))
            .ForMember(w => w.Summary, opt => opt.MapFrom(m => m.weather.ElementAt(0).description))
            .ForMember(w => w.TemperatureC, opt => opt.MapFrom(m => FromKelvinToCelsius(m.main.temp)));
    }

    private DateTime FromUnix(long unixTimeStamp)
        => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();

    private int FromKelvinToCelsius(decimal kelvin)
        => (int)(kelvin - 273.15m);
}

public interface IConverterService
{
    IEnumerable<WeatherForecast> ConvertToWeatherForecasts(OpenWeatherResponseModel responseModel);
}

public class ConverterService : IConverterService
{
    private readonly IMapper mapper;

    public ConverterService()
    {
        this.mapper = new MapperConfiguration(cfg => cfg.AddProfile<WeatherForecastMapperProfile>()).CreateMapper();
    }

    public IEnumerable<WeatherForecast> ConvertToWeatherForecasts(OpenWeatherResponseModel responseModel)
    {
        return
            this.mapper.Map<List<WeatherForecast>>(responseModel.list)
                .GroupBy(r => r.Date.DayOfWeek).Select(g => g.First());
    }
}