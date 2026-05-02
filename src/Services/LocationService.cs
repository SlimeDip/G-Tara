using G_Tara.Models;
using System.Text.Json;

namespace G_Tara.Services
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;

        public LocationService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "G-Tara-App");
        }

        public async Task<List<LocationItem>> SearchLocationsAsync(double latitude, double longitude, string category)
        {
            try
            {
                var tagMapping = new Dictionary<string, (string Key, string Value)>
                {
                    { "Hotel", ("tourism", "hotel") },
                    { "Resort", ("tourism", "resort") },
                    { "Cafe", ("amenity", "cafe") },
                    { "Restaurant", ("amenity", "restaurant|cafe|fast_food|bar|pub") },
                    { "Bar", ("amenity", "bar|pub") }
                };
                // Magdagdag nalang kayo dito kung may naisip kayo
                // May specific keywords din sa pagkakaalam ko so search nyo nalang

                var key = "amenity";
                var val = category.ToLower();
                if (tagMapping.ContainsKey(category))
                {
                    key = tagMapping[category].Key;
                    val = tagMapping[category].Value;
                }

                var delta = 0.1; // eto adjustan ng range, maganda din siguro kung may slider ng range. Di ko tanda computation pero 10km ata yan. Search nyo nalang lol
                var south = (latitude - delta).ToString(System.Globalization.CultureInfo.InvariantCulture);
                var north = (latitude + delta).ToString(System.Globalization.CultureInfo.InvariantCulture);
                var west = (longitude - delta).ToString(System.Globalization.CultureInfo.InvariantCulture);
                var east = (longitude + delta).ToString(System.Globalization.CultureInfo.InvariantCulture);
                var bbox = $"{south},{west},{north},{east}";

                var overpassQuery = $@"[out:json];
                (
                node[""{key}""~""{val}""]({bbox});
                way[""{key}""~""{val}""]({bbox});
                relation[""{key}""~""{val}""]({bbox});
                );
                out center;";
                var url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(overpassQuery)}";

                System.Diagnostics.Debug.WriteLine($"[LocationSearch] Category: {category} -> Key: {key}, Val: {val}");
                System.Diagnostics.Debug.WriteLine($"[LocationSearch] BBox: {bbox}");
                System.Diagnostics.Debug.WriteLine($"[LocationSearch] API URL: {url}");

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[LocationSearch] Error: {response.StatusCode}");
                    return new List<LocationItem> { new LocationItem { Name = "Try again (API Error)" } };
                }

                var content = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                var elements = root.GetProperty("elements");
                System.Diagnostics.Debug.WriteLine($"[LocationSearch] Processed {elements.GetArrayLength()} elements from response");

                var locations = new List<LocationItem>();

                foreach (var element in elements.EnumerateArray())
                {
                    var lat = 0.0;
                    var lon = 0.0;
                    var name = "Unknown";

                    if (!element.TryGetProperty("tags", out var tags) || !tags.TryGetProperty("name", out var nameProp))
                    {
                        continue;
                    }

                    name = nameProp.GetString() ?? "Unknown";

                    if (element.TryGetProperty("center", out var center))
                    {
                        lat = center.GetProperty("lat").GetDouble();
                        lon = center.GetProperty("lon").GetDouble();
                    }
                    else if (element.TryGetProperty("lat", out var latProp) && element.TryGetProperty("lon", out var lonProp))
                    {
                        lat = latProp.GetDouble();
                        lon = lonProp.GetDouble();
                    }
                    else
                    {
                        continue;
                    }

                    locations.Add(new LocationItem
                    {
                        Name = name,
                        Category = category,
                        Latitude = lat,
                        Longitude = lon,
                        Address = "Address not available (Skip to save API limits)"
                    });

                    if (locations.Count >= 10) break;
                }

                if (locations.Count == 0)
                {
                    locations.Add(new LocationItem { Name = "Cant find anything" });
                }

                return locations;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Location Search Error: {ex.Message}");
                return new List<LocationItem> { new LocationItem { Name = "Try again (Exception)" } };
            }
        }

        public async Task<(double latitude, double longitude)> GeocodeLocationAsync(string location)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(location)}&format=json&limit=1";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return (0, 0);
                }

                var content = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.GetArrayLength() > 0)
                {
                    var first = root[0];
                    var lat = double.Parse(first.GetProperty("lat").GetString() ?? "0");
                    var lon = double.Parse(first.GetProperty("lon").GetString() ?? "0");
                    return (lat, lon);
                }

                return (0, 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Geocoding Error: {ex.Message}");
                return (0, 0);
            }
        }

        public async Task<string> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode) return "Unknown Address";

                var content = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(content);

                if (doc.RootElement.TryGetProperty("address", out var address))
                {
                    var parts = new List<string>();
                    foreach (var prop in new[] { "road", "suburb", "city", "county" })
                    {
                        if (address.TryGetProperty(prop, out var value))
                        {
                            var val = value.GetString();
                            if (!string.IsNullOrEmpty(val) && !parts.Contains(val)) parts.Add(val);
                        }
                    }
                    return string.Join(", ", parts.Take(2));
                }
                return "Unknown Address";
            }
            catch
            {
                return "Unknown Address";
            }
        }
    }
}
