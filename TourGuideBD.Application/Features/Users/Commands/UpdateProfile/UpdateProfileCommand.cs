using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Users.Common;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<UserProfileDto>
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
    }
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateProfileCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        user.FullName = request.FullName;
        await _context.SaveChangesAsync(cancellationToken);

        var roles = await _identityService.GetUserRolesAsync(user.Id);

        return new UserProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            AvatarUrl = user.AvatarUrl,
            CreatedAt = user.CreatedAt,
            Roles = roles.ToList()
        };
    }
}