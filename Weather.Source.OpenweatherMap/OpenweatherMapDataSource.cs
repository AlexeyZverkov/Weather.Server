using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Weather.Source.Interface;
using Weather.Source.Interface.Entities;

namespace Weather.Source.OpenweatherMap
{
    public sealed class OpenweatherMapDataSource : IWeatherDataSource
    {
        private readonly string _weatherForecastEndpoint;
        private readonly string _apiKey;

        private readonly HttpClient _client = null;

        public OpenweatherMapDataSource(string weatherForecastEndpoint, string apiKey)
        {
            _weatherForecastEndpoint = weatherForecastEndpoint;
            _apiKey = apiKey;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WeatherForecast> GetWeatherForecastByCity(string city)
        {
            var result = await GetWeatherForecast($"{_weatherForecastEndpoint}?q={city}&APPID={_apiKey}");

            return result;
        }

        public async Task<WeatherForecast> GetWeatherForecastByZipCode(string zipCode)
        {
            var result = await GetWeatherForecast($"{_weatherForecastEndpoint}?zip={zipCode}&APPID={_apiKey}");

            return result;
        }

        private async Task<WeatherForecast> GetWeatherForecast(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(json);

                var data = new List<WeatherData>();

                foreach (var item in jObject["list"].Children())
                    data.Add(new WeatherData
                    {
                        DateTime = DateTimeOffset.FromUnixTimeSeconds(item["dt"].ToObject<long>()).DateTime,
                        Temperature = item["main"]["temp"].ToObject<double>(),
                        Humidity = item["main"]["humidity"].ToObject<int>(),
                        WindSpeed = item["wind"]["speed"].ToObject<int>()
                    });

                return new WeatherForecast
                {
                    City = jObject["city"]["name"].ToObject<string>(),
                    Data = data
                };
            } else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            throw new Exception($"Can't retrieve data from {url}. Response.StatusCode={response.StatusCode}");
        }
    }
}
