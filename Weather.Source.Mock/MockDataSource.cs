using System;
using System.Linq;
using System.Threading.Tasks;
using Weather.Source.Interface;
using Weather.Source.Interface.Entities;

namespace Weather.Source.Mock
{
    public class MockDataSource : IWeatherDataSource
    {
        public Task<WeatherForecast> GetWeatherForecastByCity(string city)
        {
            try
            {
                return Task.FromResult(GetWeatherForecast(city));
            }
            catch (Exception e)
            {
                return Task.FromException<WeatherForecast>(e);
            }
        }

        public Task<WeatherForecast> GetWeatherForecastByZipCode(string zipCode)
        {
            try
            {
                return Task.FromResult(GetWeatherForecast(zipCode));
            }
            catch (Exception e)
            {
                return Task.FromException<WeatherForecast>(e);
            }
        }

        private WeatherForecast GetWeatherForecast(string request)
        {
            var random = new Random();

            return new WeatherForecast()
            {
                City = $"Mock city {request.Split(',')[0]}",
                Data = Enumerable.Range(0, 6).Select(index => new WeatherData
                {
                    DateTime = DateTime.Now.AddDays(index),
                    Temperature = random.Next(253, 328),
                    Humidity = random.Next(0, 100),
                    WindSpeed = random.Next(0, 20)
                }).ToArray()
            };
        }
    }
}
