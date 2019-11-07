using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Source.Interface.Entities;

namespace Weather.Source.Interface
{
    public interface IWeatherDataSource
    {
        Task<WeatherForecast> GetWeatherForecastByCity(string city);
        Task<WeatherForecast> GetWeatherForecastByZipCode(string zipCode);
    }
}
