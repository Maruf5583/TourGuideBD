using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommand : IRequest<string>
{
    public string UserId { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}

public class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
    private static readonly string[] AllowedContentTypes =
        { "image/jpeg", "image/png", "image/webp" };
    private const long MaxFileSizeBytes = 3 * 1024 * 1024; // 3 MB

    public UploadAvatarCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.File)
            .NotNull().WithMessage("A file is required.");

        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(MaxFileSizeBytes)
                .WithMessage("Avatar must be 3MB or smaller.");

            RuleFor(x => x.File.ContentType)
                .Must(ct => AllowedContentTypes.Contains(ct))
                .WithMessage("Only JPEG, PNG, and WEBP images are allowed.");
        });
    }
}

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;
    private const string ContainerName = "user-avatars";

    public UploadAvatarCommandHandler(
        IApplicationDbContext context,
        IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<string> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        // Delete old avatar if exists
        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            await _blobStorageService.DeleteAsync(user.AvatarUrl, ContainerName, cancellationToken);
        }

        using var stream = request.File.OpenReadStream();
        var url = await _blobStorageService.UploadAsync(
            stream,
            request.File.FileName,
            request.File.ContentType,
            ContainerName,
            cancellationToken);

        user.AvatarUrl = url;
        await _context.SaveChangesAsync(cancellationToken);

        return url;
    }
}