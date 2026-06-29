using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Trip;

public class TransportType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public TransportTypeEnum Type { get; set; }

    public ICollection<TransportRate> Rates { get; set; } = new List<TransportRate>();
}