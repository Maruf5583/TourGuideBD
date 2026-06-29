namespace TourGuideBD.Application.Features.Locations.Queries.GetDistrictsByDivision;

public class DistrictDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public int DivisionId { get; set; }
}