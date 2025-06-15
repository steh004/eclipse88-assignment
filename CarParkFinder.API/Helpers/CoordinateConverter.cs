using System;

namespace CarParkFinder.API.Helper
{
    public static class CoordinateConverter
    {
        // SVY21 to WGS84 (Latitude, Longitude)
        public static (double lat, double lng) Convert(double northing, double easting)
        {
            const double a = 6378137.0; // WGS84 major axis
            const double f = 1 / 298.257223563; // WGS84 flattening
            const double oLat = 1.366666; // Origin latitude in degrees
            const double oLng = 103.833333; // Origin longitude in degrees
            const double No = 38744.572;
            const double Eo = 28001.642;
            const double k = 1.0;

            double b = a * (1 - f);
            double e2 = (2 * f) - (f * f);
            double n = (a - b) / (a + b);
            double n2 = n * n;
            double n3 = n2 * n;
            double n4 = n2 * n2;

            double G = a * (1 - n) * (1 - n2) * (1 + (9 * n2 / 4) + (225 * n4 / 64));

            double Nprime = northing - No;
            double Mo = G * oLat * Math.PI / 180.0;

            double Mprime = Mo + Nprime / k;
            double sigma = Mprime / (a * (1 - e2 / 4 - 3 * e2 * e2 / 64));

            double latRad = sigma;

            double sin2lat = Math.Sin(2 * latRad);
            double sin4lat = Math.Sin(4 * latRad);
            double sin6lat = Math.Sin(6 * latRad);

            double lat = latRad + (3 * n / 2 - 27 * n3 / 32) * sin2lat
                                 + (21 * n2 / 16 - 55 * n4 / 32) * sin4lat
                                 + (151 * n3 / 96) * sin6lat;

            double sinLat = Math.Sin(lat);
            double cosLat = Math.Cos(lat);
            double tanLat = Math.Tan(lat);

            double esinLat = 1 - e2 * sinLat * sinLat;

            double v = a / Math.Sqrt(esinLat);
            double rho = a * (1 - e2) / Math.Pow(esinLat, 1.5);
            double psi = v / rho;

            double T = tanLat * tanLat;
            double C = e2 * cosLat * cosLat / (1 - e2);
            double A = (easting - Eo) / (k * v);

            double latitude = (lat - (tanLat / (2 * rho * v)) * A * A
                + (tanLat / (24 * rho * Math.Pow(v, 3))) * (5 + 3 * T + 10 * C - 4 * C * C - 9 * e2) * Math.Pow(A, 4)) * 180.0 / Math.PI;

            double longitude = (A - (1 + 2 * T + C) * Math.Pow(A, 3) / 6
                + (5 - 2 * C + 28 * T - 3 * C * C + 8 * e2 + 24 * T * T) * Math.Pow(A, 5) / 120) / cosLat;

            longitude = oLng + longitude * 180.0 / Math.PI;

            return (latitude, longitude);
        }
    }
}
