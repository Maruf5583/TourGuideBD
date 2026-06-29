namespace TourGuideBD.Application.Common.Interfaces;

public class RouteResult
{
    public double DistanceKm { get; set; }
    public double DurationMinutes { get; set; }
    public string? Geometry { get; set; }
}

public interface IMapboxService
{
    Task<RouteResult> GetRouteAsync(
        double originLat,
        double originLng,
        double destLat,
        double destLng,
        string profile = "driving",
        CancellationToken cancellationToken = default);
}