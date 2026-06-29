using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Application.Features.Users.Common;

namespace TourGuideBD.Application.Features.Users.Queries.GetFavourites;

public class GetFavouritesQuery : IRequest<List<FavouritesByDistrictDto>>
{
    public string UserId { get; set; } = string.Empty;
}

public class GetFavouritesQueryValidator : AbstractValidator<GetFavouritesQuery>
{
    public GetFavouritesQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetFavouritesQueryHandler : IRequestHandler<GetFavouritesQuery, List<FavouritesByDistrictDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFavouritesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FavouritesByDistrictDto>> Handle(GetFavouritesQuery request, CancellationToken cancellationToken)
    {
        var favourites = await _context.FavouritePlaces
            .Where(f => f.UserId == request.UserId)
            .Include(f => f.Place).ThenInclude(p => p.District)
            .Include(f => f.Place).ThenInclude(p => p.Division)
            .Include(f => f.Place).ThenInclude(p => p.Photos)
            .Include(f => f.Place).ThenInclude(p => p.CategoryMaps).ThenInclude(cm => cm.PlaceCategory)
            .OrderByDescending(f => f.AddedAt)
            .ToListAsync(cancellationToken);

        var grouped = favourites
            .GroupBy(f => new { f.Place.DistrictId, f.Place.District.Name })
            .Select(g => new FavouritesByDistrictDto
            {
                DistrictId = g.Key.DistrictId,
                DistrictName = g.Key.Name,
                Places = g.Select(f => new FavouritePlaceDto
                {
                    FavouriteId = f.Id,
                    AddedAt = f.AddedAt,
                    Place = _mapper.Map<PlaceListItemDto>(f.Place)
                }).ToList()
            })
            .OrderBy(g => g.DistrictName)
            .ToList();

        return grouped;
    }
}