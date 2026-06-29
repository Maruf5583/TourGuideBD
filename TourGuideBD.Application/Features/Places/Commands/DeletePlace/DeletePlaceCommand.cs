using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.DeletePlace;

public class DeletePlaceCommand : IRequest<Unit>, IAuditableRequest
{
    public int Id { get; set; }

    public string ActionName => "DeletePlace";
    public string EntityName => nameof(Place);
    public string? EntityId => Id.ToString();
}

public class DeletePlaceCommandValidator : AbstractValidator<DeletePlaceCommand>
{
    public DeletePlaceCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Place), request.Id);
        }

        _context.Places.Remove(place);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}