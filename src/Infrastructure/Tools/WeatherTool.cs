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
            if (string.IsNullOrWhiteSpace(city))
            {
                return "Şehir bilgisi boş olamaz";
            }

            var apiKey = _configuration["OpenWeather:weatherApi"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric";


            try
            {
                var response= await _httpClient.GetAsync(url,cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return $"{city} şehri bulunamadı.";
                }
                Console.WriteLine($"[WeatherTool] Called with city: {city}");

                if (!response.IsSuccessStatusCode)
                {
                    return "Hava durumu servisi kullanılamıyor.";
                }

                var json= await response.Content.ReadAsStringAsync();
                var weather=JsonSerializer.Deserialize<WeatherModels>(json);

                if (weather == null) {
                    return "Hava durumu servisinden geçerli veri alınamadı.";
                
                }
                Console.WriteLine($"[WeatherTool] Response: ");

                var description = weather.Weather.FirstOrDefault()?.Description ?? "bilinmiyor";
                return $"{weather.CityName}: {weather.Main.Temperature} derece, hissedilen: {weather.Main.FeelsLike} derece, nem %{weather.Main.Humidity}, {description}";
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (JsonException)
            {
                return "Hava durumu servisinden beklenmeyen bir veri alındı.";
            }
            catch (Exception)
            {
                return "Hava durumu verisi alınırken beklenmedik bir hata oluştu.";
            }

           
        }

    }
}
