using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarParkFinder.API.Controllers;
using CarParkFinder.API.Services;
using CarParkFinder.API.Models;

namespace CarParkFinder.Tests.Controllers
{
    public class CarParksControllerTests
    {
        private readonly Mock<ICarParkService> _mockService;
        private readonly CarParksController _controller;

        public CarParksControllerTests()
        {
            _mockService = new Mock<ICarParkService>();
            _controller = new CarParksController(_mockService.Object);
        }

        [Fact]
        public async Task GetNearest_MissingLatitudeOrLongitude_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetNearest(null, 103.8);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("latitude and longitude are required.", badRequest.Value);
        }

        [Fact]
        public async Task GetNearest_ValidCoordinates_ReturnsOkResultWithData()
        {
            // Arrange
            var mockData = new List<CarParkDto>
            {
                new CarParkDto
                {
                    Address = "123 Main St",
                    Latitude = 1.3,
                    Longitude = 103.8,
                    AvailableLots = 5,
                    TotalLots = 50,
                    Distance = 100
                }
            };

            _mockService
                .Setup(s => s.GetNearestAvailableCarParks(1.3, 103.8, 1, 10))
                .ReturnsAsync(mockData);

            // Act
            var result = await _controller.GetNearest(1.3, 103.8);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<List<CarParkDto>>(okResult.Value);
            Assert.Single(data);
            Assert.Equal("123 Main St", data[0].Address);
        }
    }
}
