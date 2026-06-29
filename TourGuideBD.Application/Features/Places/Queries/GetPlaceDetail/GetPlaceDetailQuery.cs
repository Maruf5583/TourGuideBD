using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Queries.GetPlaceDetail;

public class GetPlaceDetailQuery : IRequest<PlaceDetailDto>
{
    public int PlaceId { get; set; }
}

public class GetPlaceDetailQueryValidator : AbstractValidator<GetPlaceDetailQuery>
{
    public GetPlaceDetailQueryValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class GetPlaceDetailQueryHandler : IRequestHandler<GetPlaceDetailQuery, PlaceDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlaceDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlaceDetailDto> Handle(GetPlaceDetailQuery request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .Where(p => p.Id == request.PlaceId && p.ApprovalStatus == ApprovalStatus.Approved)
            .ProjectTo<PlaceDetailDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Location.Place), request.PlaceId);
        }

        return place;
    }
}