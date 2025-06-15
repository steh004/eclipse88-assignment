using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CarParkFinder.API.Services;
using CarParkFinder.API.Data;
using CarParkFinder.API.Models;
using CarParkFinder.API.Helpers;

namespace CarParkFinder.Tests.Services
{
    public class CarParkServiceTests
    {
        private CarParkService CreateServiceWithData(List<CarPark> carParks)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.CarParks.AddRange(carParks);
            context.SaveChanges();

            return new CarParkService(context);
        }

        [Fact]
        public async Task GetNearestAvailableCarParks_ReturnsCarParks_WithAvailableLots()
        {
            // Arrange
            var carParks = new List<CarPark>
            {
                new CarPark
                {
                    Id = 1,
                    CarParkNo = "CP1", // ✅ Add this
                    Address = "123 Example St",
                    Latitude = 1.3000,
                    Longitude = 103.8000,
                    Availabilities = new List<CarParkAvailability>
                    {
                        new CarParkAvailability
                        {
                            CarParkNo = "CP1", // ✅ Match the parent CarParkNo
                            AvailableLots = 10,
                            TotalLots = 50,
                            RetrievedAt = DateTime.UtcNow
                        }
                    }
                },
                new CarPark
                {
                    Id = 2,
                    CarParkNo = "CP2",
                    Address = "456 Another Rd",
                    Latitude = 1.3100,
                    Longitude = 103.8100,
                    Availabilities = new List<CarParkAvailability>
                    {
                        new CarParkAvailability
                        {
                            CarParkNo = "CP2",
                            AvailableLots = 0,
                            TotalLots = 60,
                            RetrievedAt = DateTime.UtcNow
                        }
                    }
                }
            };

            var service = CreateServiceWithData(carParks);

            // Act
            var result = await service.GetNearestAvailableCarParks(1.3050, 103.8050, page: 1, perPage: 10);

            // Assert
            Assert.Single(result);
            Assert.Equal("123 Example St", result[0].Address);
            Assert.Equal(10, result[0].AvailableLots);
        }

        [Fact]
        public async Task GetNearestAvailableCarParks_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            var service = CreateServiceWithData(new List<CarPark>());

            // Act
            var result = await service.GetNearestAvailableCarParks(1.3000, 103.8000, 1, 10);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetNearestAvailableCarParks_Pagination_WorksCorrectly()
        {
            // Arrange
            var carParks = Enumerable.Range(1, 30).Select(i => new CarPark
            {
                Id = i,
                CarParkNo = $"CP{i}",
                Address = $"Address {i}",
                Latitude = 1.3 + (i * 0.001),
                Longitude = 103.8 + (i * 0.001),
                Availabilities = new List<CarParkAvailability>
                {
                    new CarParkAvailability
                    {
                        CarParkNo = $"CP{i}",
                        AvailableLots = 5,
                        TotalLots = 50,
                        RetrievedAt = DateTime.UtcNow
                    }
                }
            }).ToList();

            var service = CreateServiceWithData(carParks);

            // Act
            var page1 = await service.GetNearestAvailableCarParks(1.3000, 103.8000, 1, 10);
            var page2 = await service.GetNearestAvailableCarParks(1.3000, 103.8000, 2, 10);

            // Assert
            Assert.Equal(10, page1.Count);
            Assert.Equal(10, page2.Count);
            Assert.NotEqual(page1[0].Address, page2[0].Address);
        }
    }
}
