using System;

namespace Weather.Source.Interface.Entities
{
    public class WeatherData
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double TemperatureС => Temperature - 273.15;
        public int Humidity { get; set; }
        public int WindSpeed { get; set; }
    }
}
