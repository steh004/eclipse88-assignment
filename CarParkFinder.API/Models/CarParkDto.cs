namespace CarParkFinder.API.Models
{
    public class CarParkDto
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TotalLots { get; set; }
        public int AvailableLots { get; set; }
        public double Distance { get; set; }
    }
}
