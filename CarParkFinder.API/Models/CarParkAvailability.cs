using System;

namespace CarParkFinder.API.Models
{
    public class CarParkAvailability
    {
        public int Id { get; set; }  // Primary key
        public string CarParkNo { get; set; }

        public int TotalLots { get; set; }
        public int AvailableLots { get; set; }

        public DateTime RetrievedAt { get; set; }

        // Foreign key relationship
        public int CarParkId { get; set; }
        public CarPark CarPark { get; set; }
    }
}
