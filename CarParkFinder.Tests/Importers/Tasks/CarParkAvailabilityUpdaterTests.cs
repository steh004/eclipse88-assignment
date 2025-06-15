using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CarParkFinder.API.Data;
using CarParkFinder.API.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace CarParkFinder.Tests.Services
{
    public class CarParkAvailabilityUpdaterTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // fresh DB per test
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        private HttpClient GetMockHttpClient(string responseContent)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent),
                });

            return new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async Task UpdateAvailabilityAsync_ValidApiResponse_AddsAvailabilityRecord()
        {
            // Arrange
            var context = GetDbContext();

            context.CarParks.Add(new CarPark
            {
                CarParkNo = "C1",
                Id = 1,
                Address = "Test Address",           
                Latitude = 1.0,           
                Longitude = 1.0           
            });
            await context.SaveChangesAsync();

            var mockApiResponse = JsonConvert.SerializeObject(new
            {
                items = new[]
                {
                    new
                    {
                        carpark_data = new[]
                        {
                            new
                            {
                                carpark_number = "C1",
                                carpark_info = new[]
                                {
                                    new
                                    {
                                        total_lots = "100",
                                        lots_available = "60"
                                    }
                                }
                            }
                        }
                    }
                }
            });

            var httpClient = GetMockHttpClient(mockApiResponse);

            var updater = new CarParkAvailabilityUpdater(context, httpClient);

            // Act
            await updater.UpdateAvailabilityAsync();

            // Assert
            var record = await context.CarParkAvailabilities.FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal("C1", record.CarParkNo);
            Assert.Equal(100, record.TotalLots);
            Assert.Equal(60, record.AvailableLots);
            Assert.Equal(1, record.CarParkId);
        }
    }
}
