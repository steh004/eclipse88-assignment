using Xunit;
using CarParkFinder.API.Helpers;

namespace CarParkFinder.Tests
{
    public class DistanceCalculatorTests
    {
        [Fact]
        public void CalculateDistance_ReturnsExpectedDistance()
        {
            // Arrange
            double lat1 = 1.3000;
            double lon1 = 103.8000;
            double lat2 = 1.3010;
            double lon2 = 103.8010;

            double expectedDistance = 157.2;
            double tolerance = 1.0; 

            // Act
            var result = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);

            // Assert
            Assert.InRange(result, expectedDistance - tolerance, expectedDistance + tolerance);
        }
    }
}
