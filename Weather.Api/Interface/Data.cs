using System;
using System.Text.Json.Serialization;

namespace Weather.Api.Interface
{
    public class Data
    {
        [JsonPropertyName("dt")]
        public DateTime DateTime { get; set; }
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
        [JsonPropertyName("wind_speed")]
        public int WindSpeed { get; set; }
    }
}
