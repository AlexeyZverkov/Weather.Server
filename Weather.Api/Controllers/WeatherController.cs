using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Weather.Api.Interface;
using Weather.Source.Interface;
using Weather.Source.Interface.Entities;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherDataSource _weather;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public WeatherController(ILogger<WeatherController> logger, IWeatherDataSource weather, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _weather = weather;
            _cache = cache;
            _configuration = configuration;
        }

        [Route("forecast")]
        [HttpGet]
        public async Task<Forecast> GetWeatherForecast([FromQuery(Name = "city")]string city = null, [FromQuery(Name = "zipCode")]string zipCode = null)
        {
            if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(zipCode))
            {
                string errorMessage = "city and zipCode url parameters are missing";

                _logger.LogError(errorMessage);

                throw new ArgumentException(errorMessage);
            }

            var cacheKey = GetType().Name + "_" + (string.IsNullOrWhiteSpace(city) ? zipCode : city);

            try
            {
                var forecast = await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_configuration.GetValue<int>("CacheExpiration"));

                    var result = string.IsNullOrWhiteSpace(city) ? await _weather.GetWeatherForecastByZipCode(zipCode) : await _weather.GetWeatherForecastByCity(city);

                    return result == null ? null :
                    new Forecast()
                    {
                        City = result.City,
                        Data =
                        result.Data.GroupBy(y => y.DateTime.Date)
                            .Select(g => new Data {
                                    DateTime = g.Min(i => i.DateTime),
                                    Temperature = Math.Round(g.Average(i => i.TemperatureС)),
                                    Humidity = (int)Math.Round(g.Average(i => i.Humidity)),
                                    WindSpeed = (int)Math.Round(g.Average(i => i.WindSpeed))
                                }
                            )
                    };
                });

                return forecast;
            }
            catch (Exception e)
            {
                string errorMessage = "Can't retrieve data by " + (string.IsNullOrWhiteSpace(city) ? $"ZIP code {zipCode}" : $"city { city}");

                _logger.LogError(e, errorMessage);

                throw new Exception(errorMessage);
            }
        }
    }
}
