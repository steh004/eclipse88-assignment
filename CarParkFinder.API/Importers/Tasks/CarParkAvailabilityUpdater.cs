using CarParkFinder.API.Models;
using CarParkFinder.API.Data;
using Newtonsoft.Json.Linq;

public class CarParkAvailabilityUpdater
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public CarParkAvailabilityUpdater(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task UpdateAvailabilityAsync()
    {
        var response = await _httpClient.GetAsync("https://api.data.gov.sg/v1/transport/carpark-availability");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(jsonString);
        var items = json["items"]?[0]?["carpark_data"];

        if (items == null) return;

        foreach (var item in items)
        {
            string carParkNo = item["carpark_number"]?.ToString();
            var info = item["carpark_info"]?[0];
            if (info == null) continue;

            int.TryParse(info["total_lots"]?.ToString(), out int totalLots);
            int.TryParse(info["lots_available"]?.ToString(), out int availableLots);

            var carPark = _context.CarParks.FirstOrDefault(cp => cp.CarParkNo == carParkNo);
            if (carPark != null)
            {
                var availability = new CarParkAvailability
                {
                    CarParkNo = carParkNo,
                    CarParkId = carPark.Id,
                    TotalLots = totalLots,
                    AvailableLots = availableLots,
                    RetrievedAt = DateTime.UtcNow
                };

                _context.CarParkAvailabilities.Add(availability);
            }
        }

        await _context.SaveChangesAsync();
    }
}
