using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Weather.Api.Interface
{
    public class Forecast
    {
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("data")]
        public IEnumerable<Data> Data { get; set; }
    }
}
