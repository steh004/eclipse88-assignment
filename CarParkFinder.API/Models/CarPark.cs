using System.Collections.Generic;

namespace CarParkFinder.API.Models
{
    public class CarPark
    {
        public int Id { get; set; }  // Primary key
        public string CarParkNo { get; set; }
        public string Address { get; set; }

        // Converted coordinates
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Navigation property
        public ICollection<CarParkAvailability> Availabilities { get; set; }
    }
}
