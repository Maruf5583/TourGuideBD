using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Users.Commands.UnsaveDistrict;

public class UnsaveDistrictCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int DistrictId { get; set; }
}

public class UnsaveDistrictCommandValidator : AbstractValidator<UnsaveDistrictCommand>
{
    public UnsaveDistrictCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DistrictId).GreaterThan(0);
    }
}

public class UnsaveDistrictCommandHandler : IRequestHandler<UnsaveDistrictCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UnsaveDistrictCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UnsaveDistrictCommand request, CancellationToken cancellationToken)
    {
        var saved = await _context.SavedDistricts
            .FirstOrDefaultAsync(s => s.UserId == request.UserId && s.DistrictId == request.DistrictId, cancellationToken);

        if (saved != null)
        {
            _context.SavedDistricts.Remove(saved);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}