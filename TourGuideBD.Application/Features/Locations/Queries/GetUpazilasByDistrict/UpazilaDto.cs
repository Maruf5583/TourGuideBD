namespace TourGuideBD.Application.Features.Locations.Queries.GetUpazilasByDistrict;

public class UpazilaDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public int DistrictId { get; set; }
}