using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Locations.Queries.GetDivisions;

public class GetDivisionsQuery : IRequest<List<DivisionDto>>
{
}

public class GetDivisionsQueryHandler : IRequestHandler<GetDivisionsQuery, List<DivisionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDivisionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DivisionDto>> Handle(GetDivisionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Divisions
            .OrderBy(d => d.Name)
            .ProjectTo<DivisionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}