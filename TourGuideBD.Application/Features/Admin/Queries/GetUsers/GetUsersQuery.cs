using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Admin.Common;

namespace TourGuideBD.Application.Features.Admin.Queries.GetUsers;

public class GetUsersQuery : IRequest<PaginatedList<UserManagementDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchTerm { get; set; }
}

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserManagementDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<PaginatedList<UserManagementDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(u => u.Email!.ToLower().Contains(term) || u.FullName.ToLower().Contains(term));
        }

        var paged = await PaginatedList<Domain.Entities.Users.ApplicationUser>.CreateAsync(
            query.OrderBy(u => u.FullName), request.PageNumber, request.PageSize, cancellationToken);

        var items = new List<UserManagementDto>();

        foreach (var user in paged.Items)
        {
            var roles = await _identityService.GetUserRolesAsync(user.Id);

            items.Add(new UserManagementDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                IsBanned = user.IsBanned,
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt
            });
        }

        return new PaginatedList<UserManagementDto>(items, paged.TotalCount, request.PageNumber, request.PageSize);
    }
}