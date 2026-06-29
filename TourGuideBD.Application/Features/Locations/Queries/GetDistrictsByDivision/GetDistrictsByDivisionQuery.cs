using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Locations.Queries.GetDistrictsByDivision;

public class GetDistrictsByDivisionQuery : IRequest<List<DistrictDto>>
{
    public int DivisionId { get; set; }
}

public class GetDistrictsByDivisionQueryValidator : AbstractValidator<GetDistrictsByDivisionQuery>
{
    public GetDistrictsByDivisionQueryValidator()
    {
        RuleFor(x => x.DivisionId).GreaterThan(0);
    }
}

public class GetDistrictsByDivisionQueryHandler : IRequestHandler<GetDistrictsByDivisionQuery, List<DistrictDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDistrictsByDivisionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DistrictDto>> Handle(GetDistrictsByDivisionQuery request, CancellationToken cancellationToken)
    {
        var divisionExists = await _context.Divisions
            .AnyAsync(d => d.Id == request.DivisionId, cancellationToken);

        if (!divisionExists)
        {
            throw new NotFoundException(nameof(Domain.Entities.Location.Division), request.DivisionId);
        }

        return await _context.Districts
            .Where(d => d.DivisionId == request.DivisionId)
            .OrderBy(d => d.Name)
            .ProjectTo<DistrictDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}