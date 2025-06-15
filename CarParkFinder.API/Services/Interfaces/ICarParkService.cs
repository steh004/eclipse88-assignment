using CarParkFinder.API.Models;

namespace CarParkFinder.API.Services
{
    public interface ICarParkService
    {
        Task<List<CarParkDto>> GetNearestAvailableCarParks(double latitude, double longitude, int page, int perPage);
    }

}
