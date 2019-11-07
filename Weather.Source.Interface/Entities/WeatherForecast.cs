using System.Collections.Generic;

namespace Weather.Source.Interface.Entities
{
    public class WeatherForecast
    {
        public string City { get; set; }
        public IEnumerable<WeatherData> Data { get; set; }
    }
}
