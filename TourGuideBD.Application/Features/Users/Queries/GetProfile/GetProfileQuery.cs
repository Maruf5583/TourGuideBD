using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Users.Common;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Users.Queries.GetProfile;

public class GetProfileQuery : IRequest<UserProfileDto>
{
    public string UserId { get; set; } = string.Empty;
}

public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
{
    public GetProfileQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetProfileQueryHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

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