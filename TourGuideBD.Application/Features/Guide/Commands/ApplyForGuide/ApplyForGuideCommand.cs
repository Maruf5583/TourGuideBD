using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.ApplyForGuide;

public class ApplyForGuideCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;

    // Personal
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    // Documents
    public string NidFrontPhotoUrl { get; set; } = string.Empty;
    public string NidBackPhotoUrl { get; set; } = string.Empty;
    public string DobCertificatePhotoUrl { get; set; } = string.Empty;

    // Professional
    public int ExperienceYears { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<string> Specialities { get; set; } = new();
    public List<int> OperatingDistrictIds { get; set; } = new();
    public string? CertificateUrl { get; set; }
}

public class ApplyForGuideCommandValidator : AbstractValidator<ApplyForGuideCommand>
{
    public ApplyForGuideCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(dob => DateTime.UtcNow.Year - dob.Year >= 18)
            .WithMessage("You must be at least 18 years old.");

        RuleFor(x => x.NidFrontPhotoUrl)
            .NotEmpty().WithMessage("NID front photo is required.");

        RuleFor(x => x.NidBackPhotoUrl)
            .NotEmpty().WithMessage("NID back photo is required.");

        RuleFor(x => x.DobCertificatePhotoUrl)
            .NotEmpty().WithMessage("Date of birth certificate photo is required.");

        RuleFor(x => x.ProfilePhotoUrl).NotEmpty();
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Bio).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ExperienceYears).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Languages)
            .NotEmpty().WithMessage("At least one language required.");

        RuleFor(x => x.OperatingDistrictIds)
            .NotEmpty().WithMessage("At least one district required.");
    }
}

public class ApplyForGuideCommandHandler : IRequestHandler<ApplyForGuideCommand, int>
{
    private readonly IApplicationDbContext _context;

    public ApplyForGuideCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(ApplyForGuideCommand request, CancellationToken cancellationToken)
    {
        var alreadyApplied = await _context.GuideApplications
            .AnyAsync(g => g.UserId == request.UserId &&
                (g.Status == GuideApplicationStatus.Pending ||
                 g.Status == GuideApplicationStatus.UnderReview ||
                 g.Status == GuideApplicationStatus.Approved),
                cancellationToken);

        if (alreadyApplied)
        {
            throw new DomainValidationException("Application",
                "You already have a pending or approved guide application.");
        }

        var application = new GuideApplication
        {
            UserId = request.UserId,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            ProfilePhotoUrl = request.ProfilePhotoUrl,
            Address = request.Address,
            Bio = request.Bio,
            DateOfBirth = request.DateOfBirth,
            NidFrontPhotoUrl = request.NidFrontPhotoUrl,
            NidBackPhotoUrl = request.NidBackPhotoUrl,
            DobCertificatePhotoUrl = request.DobCertificatePhotoUrl,
            ExperienceYears = request.ExperienceYears,
            Languages = string.Join(",", request.Languages),
            Specialities = string.Join(",", request.Specialities),
            OperatingDistrictIds = JsonSerializer.Serialize(request.OperatingDistrictIds),
            CertificateUrl = request.CertificateUrl,
            Status = GuideApplicationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.GuideApplications.Add(application);
        await _context.SaveChangesAsync(cancellationToken);

        return application.Id;
    }
}