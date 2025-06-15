using Microsoft.EntityFrameworkCore;
using CarParkFinder.API.Data;
using CarParkFinder.API.Models;
using CarParkFinder.API.Helpers;

namespace CarParkFinder.API.Services
{
    public class CarParkService : ICarParkService
    {
        private readonly AppDbContext _context;

        public CarParkService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarParkDto>> GetNearestAvailableCarParks(double latitude, double longitude, int page, int perPage)
        {
            var recentTime = DateTime.UtcNow.AddMinutes(-10);

            var carParks = await _context.CarParks
                .Include(cp => cp.Availabilities)
                .ToListAsync();

            var result = carParks
                .Select(cp => new
                {
                    CarPark = cp,
                    Availability = cp.Availabilities
                        .OrderByDescending(a => a.RetrievedAt)
                        .FirstOrDefault()
                })
                .Where(x => x.Availability != null && x.Availability.AvailableLots > 0)
                .Select(x => new
                {
                    x.CarPark,
                    x.Availability,
                    Distance = DistanceCalculator.CalculateDistance(latitude, longitude, x.CarPark.Latitude, x.CarPark.Longitude)
                })
                .OrderBy(x => x.Distance)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(x => new CarParkDto
                {
                    Address = x.CarPark.Address,
                    Latitude = x.CarPark.Latitude,
                    Longitude = x.CarPark.Longitude,
                    TotalLots = x.Availability.TotalLots,
                    AvailableLots = x.Availability.AvailableLots
                })
                .ToList();

            return result;
        }
    }
}
