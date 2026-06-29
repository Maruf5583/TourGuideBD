using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.DeletePlacePhoto;

public class DeletePlacePhotoCommand : IRequest<Unit>
{
    public int PhotoId { get; set; }
}

public class DeletePlacePhotoCommandValidator : AbstractValidator<DeletePlacePhotoCommand>
{
    public DeletePlacePhotoCommandValidator()
    {
        RuleFor(x => x.PhotoId).GreaterThan(0);
    }
}

public class DeletePlacePhotoCommandHandler : IRequestHandler<DeletePlacePhotoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;
    private const string ContainerName = "place-photos";

    public DeletePlacePhotoCommandHandler(
        IApplicationDbContext context,
        IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<Unit> Handle(
        DeletePlacePhotoCommand request,
        CancellationToken cancellationToken)
    {
        var photo = await _context.PlacePhotos
            .FirstOrDefaultAsync(p => p.Id == request.PhotoId, cancellationToken);

        if (photo == null)
        {
            throw new NotFoundException("PlacePhoto", request.PhotoId);
        }

        // Storage থেকে delete করো
        await _blobStorageService.DeleteAsync(photo.Url, ContainerName, cancellationToken);

        // DB থেকে delete করো
        _context.PlacePhotos.Remove(photo);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}