using Microsoft.AspNetCore.Mvc;
using CarParkFinder.API.Services;

namespace CarParkFinder.API.Controllers
{
    [ApiController]
    [Route("carparks")]
    public class CarParksController : ControllerBase
    {
        private readonly ICarParkService _service;

        public CarParksController(ICarParkService service)
        {
            _service = service;
        }

        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearest([FromQuery] double? latitude, [FromQuery] double? longitude, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            if (latitude == null || longitude == null)
            {
                return BadRequest("latitude and longitude are required.");
            }

            var result = await _service.GetNearestAvailableCarParks(latitude.Value, longitude.Value, page, per_page);
            return Ok(result);
        }
    }
}
