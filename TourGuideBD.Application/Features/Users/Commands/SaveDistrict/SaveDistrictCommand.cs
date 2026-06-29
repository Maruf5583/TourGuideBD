using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Users.Commands.SaveDistrict;

public class SaveDistrictCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int DistrictId { get; set; }
}

public class SaveDistrictCommandValidator : AbstractValidator<SaveDistrictCommand>
{
    public SaveDistrictCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DistrictId).GreaterThan(0);
    }
}

public class SaveDistrictCommandHandler : IRequestHandler<SaveDistrictCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SaveDistrictCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SaveDistrictCommand request, CancellationToken cancellationToken)
    {
        var districtExists = await _context.Districts.AnyAsync(d => d.Id == request.DistrictId, cancellationToken);
        if (!districtExists)
        {
            throw new NotFoundException(nameof(District), request.DistrictId);
        }

        var alreadySaved = await _context.SavedDistricts
            .AnyAsync(s => s.UserId == request.UserId && s.DistrictId == request.DistrictId, cancellationToken);

        if (alreadySaved)
        {
            return Unit.Value;
        }

        _context.SavedDistricts.Add(new SavedDistrict
        {
            UserId = request.UserId,
            DistrictId = request.DistrictId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}