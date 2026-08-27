using Application.Chat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Infrastructure.Tools
{
    public class WeatherTool
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherTool(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            
        }

        [Description("Get the weather for given city.")]
        public async Task<string> GetWeather([Description("The city to get the weather for.")] string city, CancellationToken cancellationToken)
        {
            var apiKey = _configuration["OpenWeather:weatherApi"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";


            try
            {
                var response= await _httpClient.GetAsync(url,cancellationToken);
                response.EnsureSuccessStatusCode();
                var json= await response.Content.ReadAsStringAsync();
                var weather=JsonSerializer.Deserialize<WeatherModels>(json);

                var description = weather.Weather.FirstOrDefault()?.Description ?? "bilinmiyor";
                return $"{weather.CityName}: {weather.Main.Temperature} derece, hissedilen: {weather.Main.FeelsLike} derece, nem %{weather.Main.Humidity}, {description}";
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

           
        }

    }
}
