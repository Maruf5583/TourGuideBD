using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Locations.Queries.GetUpazilasByDistrict;

public class GetUpazilasByDistrictQuery : IRequest<List<UpazilaDto>>
{
    public int DistrictId { get; set; }
}

public class GetUpazilasByDistrictQueryValidator : AbstractValidator<GetUpazilasByDistrictQuery>
{
    public GetUpazilasByDistrictQueryValidator()
    {
        RuleFor(x => x.DistrictId).GreaterThan(0);
    }
}

public class GetUpazilasByDistrictQueryHandler : IRequestHandler<GetUpazilasByDistrictQuery, List<UpazilaDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUpazilasByDistrictQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UpazilaDto>> Handle(GetUpazilasByDistrictQuery request, CancellationToken cancellationToken)
    {
        var districtExists = await _context.Districts
            .AnyAsync(d => d.Id == request.DistrictId, cancellationToken);

        if (!districtExists)
        {
            throw new NotFoundException(nameof(Domain.Entities.Location.District), request.DistrictId);
        }

        return await _context.Upazilas
            .Where(u => u.DistrictId == request.DistrictId)
            .OrderBy(u => u.Name)
            .ProjectTo<UpazilaDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}