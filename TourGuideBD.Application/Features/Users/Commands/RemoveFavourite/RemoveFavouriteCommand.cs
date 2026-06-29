using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Users.Commands.RemoveFavourite;

public class RemoveFavouriteCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int PlaceId { get; set; }
}

public class RemoveFavouriteCommandValidator : AbstractValidator<RemoveFavouriteCommand>
{
    public RemoveFavouriteCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class RemoveFavouriteCommandHandler : IRequestHandler<RemoveFavouriteCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RemoveFavouriteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveFavouriteCommand request, CancellationToken cancellationToken)
    {
        var favourite = await _context.FavouritePlaces
            .FirstOrDefaultAsync(f => f.UserId == request.UserId && f.PlaceId == request.PlaceId, cancellationToken);

        if (favourite != null)
        {
            _context.FavouritePlaces.Remove(favourite);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}