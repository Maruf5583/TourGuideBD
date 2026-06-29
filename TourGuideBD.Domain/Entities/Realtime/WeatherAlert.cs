using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Realtime;

public class WeatherAlert : AuditableEntity
{
    public int DistrictId { get; set; }
    public District District { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info"; // Info, Warning, Severe

    public DateTime ValidUntil { get; set; }
    public bool IsActive { get; set; } = true;
}