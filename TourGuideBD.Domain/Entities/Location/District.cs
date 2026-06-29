using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Location;

public class District : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;

    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;

    public ICollection<Upazila> Upazilas { get; set; } = new List<Upazila>();
    public ICollection<Place> Places { get; set; } = new List<Place>();
}