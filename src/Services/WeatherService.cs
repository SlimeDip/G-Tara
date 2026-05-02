using G_Tara.Models;

namespace G_Tara.Services
{
    public class WeatherService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
            _httpClient = new HttpClient();
        }

        public async Task<WeatherData?> GetWeatherAsync(double latitude, double longitude, DateTime date)
        {
            try
            {
                var forecastDate = date.ToString("yyyy-MM-dd");
                var forecastUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";

                var response = await _httpClient.GetAsync(forecastUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var doc = System.Text.Json.JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("list", out var list))
                    {
                        foreach (var item in list.EnumerateArray())
                        {
                            var dtTxt = item.GetProperty("dt_txt").GetString();
                            if (dtTxt != null && dtTxt.StartsWith(forecastDate))
                            {
                                var main = item.GetProperty("main");
                                var weather0 = item.GetProperty("weather")[0];
                                var wind = item.GetProperty("wind");

                                return new WeatherData
                                {
                                    Temperature = Math.Round(main.GetProperty("temp").GetDouble(), 0),
                                    Description = weather0.GetProperty("description").GetString() ?? "N/A",
                                    Humidity = main.GetProperty("humidity").GetInt32(),
                                    WindSpeed = Math.Round(wind.GetProperty("speed").GetDouble() * 3.6, 0), // m/s to km/h
                                    IconUrl = weather0.GetProperty("icon").GetString() ?? ""
                                };
                            }
                        }
                    }
                }

                var currentUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric";
                var currentResponse = await _httpClient.GetAsync(currentUrl);
                var currentContent = await currentResponse.Content.ReadAsStringAsync();

                if (!currentResponse.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"OpenWeather API Error: {currentResponse.StatusCode} - {currentContent}");
                    return null;
                }

                var currentDoc = System.Text.Json.JsonDocument.Parse(currentContent);
                var currentRoot = currentDoc.RootElement;

                var currentMain = currentRoot.GetProperty("main");
                var currentWeather0 = currentRoot.GetProperty("weather")[0];
                var currentWind = currentRoot.GetProperty("wind");

                return new WeatherData
                {
                    Temperature = Math.Round(currentMain.GetProperty("temp").GetDouble(), 0),
                    Description = currentWeather0.GetProperty("description").GetString() ?? "N/A",
                    Humidity = currentMain.GetProperty("humidity").GetInt32(),
                    WindSpeed = Math.Round(currentWind.GetProperty("speed").GetDouble() * 3.6, 0),
                    IconUrl = currentWeather0.GetProperty("icon").GetString() ?? ""
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Weather API Error: {ex.Message}");
                return null;
            }
        }
    }
}
