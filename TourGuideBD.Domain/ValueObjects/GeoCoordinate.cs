namespace TourGuideBD.Domain.ValueObjects;

public class GeoCoordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public GeoCoordinate() { }

    public GeoCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    // Haversine formula - returns distance in KM
    public double DistanceToInKm(GeoCoordinate other)
    {
        const double earthRadiusKm = 6371.0;

        var dLat = ToRadians(other.Latitude - Latitude);
        var dLon = ToRadians(other.Longitude - Longitude);

        var lat1 = ToRadians(Latitude);
        var lat2 = ToRadians(other.Latitude);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2) *
                Math.Cos(lat1) * Math.Cos(lat2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }

    private static double ToRadians(double degrees) => degrees * (Math.PI / 180);
}