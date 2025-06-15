using CsvHelper;
using CsvHelper.Configuration;
using CarParkFinder.API.Models;
using CarParkFinder.API.Data;
using System.Globalization;
using CarParkFinder.API.Helper;

namespace CarParkFinder.API.Services
{
    public class CarParkCsvImporter
    {
        private readonly AppDbContext _context;

        public CarParkCsvImporter(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFiles", "HDBCarparkInformation.csv");
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            // ✅ Register the mapping
            csv.Context.RegisterClassMap<CarParkCsvRecordMap>();

            var records = csv.GetRecords<CarParkCsvRecord>();

            foreach (var record in records)
            {
                if (!_context.CarParks.Any(p => p.CarParkNo == record.CarParkNo))
                {
                    var (lat, lng) = CoordinateConverter.Convert(double.Parse(record.YCoord), double.Parse(record.XCoord));

                     Console.WriteLine($"Inserted: {record.CarParkNo}, Lat: {lat}, Lng: {lng}");

                    var carPark = new CarPark
                    {
                        CarParkNo = record.CarParkNo,
                        Address = record.Address,
                        Latitude = lat,
                        Longitude = lng
                    };

                    _context.CarParks.Add(carPark);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Your existing class for reading CSV fields
        private class CarParkCsvRecord
        {
            public string CarParkNo { get; set; }
            public string Address { get; set; }
            public string XCoord { get; set; }
            public string YCoord { get; set; }
        }

        // ✅ Add this below CarParkCsvRecord
        private sealed class CarParkCsvRecordMap : ClassMap<CarParkCsvRecord>
        {
            public CarParkCsvRecordMap()
            {
                Map(m => m.CarParkNo).Name("car_park_no");
                Map(m => m.Address).Name("address");
                Map(m => m.XCoord).Name("x_coord");
                Map(m => m.YCoord).Name("y_coord");
            }
        }
    }
}
