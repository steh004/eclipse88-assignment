using Xunit;
using CarParkFinder.API.Helper;

namespace CarParkFinder.Tests.Helper
{
    public class CoordinateConverterTests
    {
        [Fact]
        public void Convert_KnownSVY21Point_ReturnsExpectedLatLng()
        {
            // This is the origin point in SVY21
            var (lat, lng) = CoordinateConverter.Convert(38744.572, 28001.642);

            Assert.InRange(lat, 1.3735, 1.3736); 
            Assert.InRange(lng, 103.8333, 103.8335);
        }

        [Fact]
        public void Convert_OffsetFromOrigin_ReturnsDifferentLatLng()
        {
            double northing = 40000;  // +1250m north
            double easting = 29000;   // +1000m east

            var (lat, lng) = CoordinateConverter.Convert(northing, easting);

            Assert.True(lat > 1.3666);
            Assert.True(lng > 103.8333);
        }

        [Fact]
        public void Convert_NegativeValues_ReturnsLatLng()
        {
            double northing = -1000;
            double easting = -1000;

            var (lat, lng) = CoordinateConverter.Convert(northing, easting);

            Assert.InRange(lat, -90, 90);
            Assert.InRange(lng, -180, 180);
        }
    }
}
