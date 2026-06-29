using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.SetCoverPhoto;

public class SetCoverPhotoCommand : IRequest<Unit>
{
    public int PlaceId { get; set; }
    public int PhotoId { get; set; }
}

public class SetCoverPhotoCommandValidator : AbstractValidator<SetCoverPhotoCommand>
{
    public SetCoverPhotoCommandValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
        RuleFor(x => x.PhotoId).GreaterThan(0);
    }
}

public class SetCoverPhotoCommandHandler : IRequestHandler<SetCoverPhotoCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SetCoverPhotoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        SetCoverPhotoCommand request,
        CancellationToken cancellationToken)
    {
        var photos = await _context.PlacePhotos
            .Where(p => p.PlaceId == request.PlaceId)
            .ToListAsync(cancellationToken);

        if (!photos.Any())
        {
            throw new NotFoundException("PlacePhoto", request.PlaceId);
        }

        var targetPhoto = photos.FirstOrDefault(p => p.Id == request.PhotoId);
        if (targetPhoto == null)
        {
            throw new NotFoundException("PlacePhoto", request.PhotoId);
        }

        // সব photo এর IsCover false করো
        foreach (var photo in photos)
        {
            photo.IsCover = false;
        }

        // Target photo টা cover করো
        targetPhoto.IsCover = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}