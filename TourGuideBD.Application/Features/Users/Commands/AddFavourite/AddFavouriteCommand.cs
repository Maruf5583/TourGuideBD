using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Users.Commands.AddFavourite;

public class AddFavouriteCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int PlaceId { get; set; }
}

public class AddFavouriteCommandValidator : AbstractValidator<AddFavouriteCommand>
{
    public AddFavouriteCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public AddFavouriteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
    {
        var placeExists = await _context.Places
            .AnyAsync(p => p.Id == request.PlaceId && p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);

        if (!placeExists)
        {
            throw new NotFoundException(nameof(Place), request.PlaceId);
        }

        var alreadyFavourite = await _context.FavouritePlaces
            .AnyAsync(f => f.UserId == request.UserId && f.PlaceId == request.PlaceId, cancellationToken);

        if (alreadyFavourite)
        {
            return Unit.Value; // idempotent - already favourited
        }

        _context.FavouritePlaces.Add(new FavouritePlace
        {
            UserId = request.UserId,
            PlaceId = request.PlaceId,
            AddedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}