using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Guide.Queries.GetPendingApplications;

public class GuideApplicationDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string NidFrontPhotoUrl { get; set; } = string.Empty;
    public string NidBackPhotoUrl { get; set; } = string.Empty;
    public string DobCertificatePhotoUrl { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string Languages { get; set; } = string.Empty;
    public string Specialities { get; set; } = string.Empty;
    public string? CertificateUrl { get; set; }
    public string OperatingDistrictIds { get; set; } = string.Empty;
    public GuideApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetPendingApplicationsQuery : IRequest<PaginatedList<GuideApplicationDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public GuideApplicationStatus? Status { get; set; }
}

public class GetPendingApplicationsQueryHandler
    : IRequestHandler<GetPendingApplicationsQuery, PaginatedList<GuideApplicationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPendingApplicationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GuideApplicationDto>> Handle(
        GetPendingApplicationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.GuideApplications.AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(g => g.Status == request.Status.Value);
        }

        var projected = query
            .OrderBy(g => g.CreatedAt)
            .Select(g => new GuideApplicationDto
            {
                Id = g.Id,
                UserId = g.UserId,
                FullName = g.FullName,
                PhoneNumber = g.PhoneNumber,
                ProfilePhotoUrl = g.ProfilePhotoUrl,
                NidFrontPhotoUrl = g.NidFrontPhotoUrl,
                NidBackPhotoUrl = g.NidBackPhotoUrl,
                DobCertificatePhotoUrl = g.DobCertificatePhotoUrl,
                DateOfBirth = g.DateOfBirth,
                Address = g.Address,
                Bio = g.Bio,
                ExperienceYears = g.ExperienceYears,
                Languages = g.Languages,
                Specialities = g.Specialities,
                CertificateUrl = g.CertificateUrl,
                OperatingDistrictIds = g.OperatingDistrictIds,
                Status = g.Status,
                CreatedAt = g.CreatedAt
            });

        return await PaginatedList<GuideApplicationDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}