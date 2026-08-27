using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Tools
{
    public class WeatherModels
    {
        [JsonPropertyName("name")]
        public string CityName { get; set; } = "";

        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; } = new();

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; } = new();
    }


    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
    }


    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }
}
