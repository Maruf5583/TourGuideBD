using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Infrastructure.ExternalServices;

public class MapboxService : IMapboxService
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;

    public MapboxService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _accessToken = configuration["Mapbox:AccessToken"]
            ?? throw new InvalidOperationException("Mapbox:AccessToken is not configured.");
    }

    public async Task<RouteResult> GetRouteAsync(double originLat, double originLng, double destLat, double destLng, string profile = "driving", CancellationToken cancellationToken = default)
    {
        // Mapbox Directions API: /directions/v5/{profile}/{coordinates}
        // coordinates format: lng,lat;lng,lat
        var url = $"https://api.mapbox.com/directions/v5/mapbox/{profile}/" +
                  $"{originLng},{originLat};{destLng},{destLat}" +
                  $"?access_token={_accessToken}&geometries=geojson&overview=simplified";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        var routes = doc.RootElement.GetProperty("routes");

        if (routes.GetArrayLength() == 0)
        {
            throw new InvalidOperationException("No route found between the given coordinates.");
        }

        var firstRoute = routes[0];

        var distanceMeters = firstRoute.GetProperty("distance").GetDouble();
        var durationSeconds = firstRoute.GetProperty("duration").GetDouble();

        string? geometry = null;
        if (firstRoute.TryGetProperty("geometry", out var geometryElement))
        {
            geometry = geometryElement.GetRawText();
        }

        return new RouteResult
        {
            DistanceKm = Math.Round(distanceMeters / 1000.0, 2),
            DurationMinutes = Math.Round(durationSeconds / 60.0, 2),
            Geometry = geometry
        };
    }
}