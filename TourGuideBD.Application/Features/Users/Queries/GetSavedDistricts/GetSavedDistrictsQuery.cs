using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Users.Common;

namespace TourGuideBD.Application.Features.Users.Queries.GetSavedDistricts;

public class GetSavedDistrictsQuery : IRequest<List<SavedDistrictDto>>
{
    public string UserId { get; set; } = string.Empty;
}

public class GetSavedDistrictsQueryValidator : AbstractValidator<GetSavedDistrictsQuery>
{
    public GetSavedDistrictsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetSavedDistrictsQueryHandler : IRequestHandler<GetSavedDistrictsQuery, List<SavedDistrictDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSavedDistrictsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SavedDistrictDto>> Handle(GetSavedDistrictsQuery request, CancellationToken cancellationToken)
    {
        return await _context.SavedDistricts
            .Where(s => s.UserId == request.UserId)
            .Include(s => s.District)
            .OrderBy(s => s.District.Name)
            .Select(s => new SavedDistrictDto
            {
                DistrictId = s.DistrictId,
                DistrictName = s.District.Name,
                DistrictNameBn = s.District.NameBn
            })
            .ToListAsync(cancellationToken);
    }
}