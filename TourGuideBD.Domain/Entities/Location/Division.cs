using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Location;

public class Division : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;

    public ICollection<District> Districts { get; set; } = new List<District>();
}