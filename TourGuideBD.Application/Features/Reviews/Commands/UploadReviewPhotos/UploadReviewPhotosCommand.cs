using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Reviews.Commands.UploadReviewPhotos;

public class UploadReviewPhotosCommand : IRequest<List<string>>
{
    public List<IFormFile> Files { get; set; } = new();
}

public class UploadReviewPhotosCommandValidator : AbstractValidator<UploadReviewPhotosCommand>
{
    private static readonly string[] AllowedContentTypes =
        { "image/jpeg", "image/png", "image/webp" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public UploadReviewPhotosCommandValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("At least one photo is required.")
            .Must(f => f.Count <= 5).WithMessage("Maximum 5 photos allowed.");

        RuleForEach(x => x.Files).ChildRules(file =>
        {
            file.RuleFor(f => f.Length)
                .LessThanOrEqualTo(MaxFileSizeBytes)
                .WithMessage("Each photo must be 5MB or smaller.");

            file.RuleFor(f => f.ContentType)
                .Must(ct => AllowedContentTypes.Contains(ct))
                .WithMessage("Only JPEG, PNG, and WEBP images are allowed.");
        });
    }
}

public class UploadReviewPhotosCommandHandler : IRequestHandler<UploadReviewPhotosCommand, List<string>>
{
    private readonly IBlobStorageService _blobStorageService;
    private const string ContainerName = "review-photos";

    public UploadReviewPhotosCommandHandler(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<List<string>> Handle(
        UploadReviewPhotosCommand request,
        CancellationToken cancellationToken)
    {
        var urls = new List<string>();

        foreach (var file in request.Files.Take(5))
        {
            using var stream = file.OpenReadStream();
            var url = await _blobStorageService.UploadAsync(
                stream,
                file.FileName,
                file.ContentType,
                ContainerName,
                cancellationToken);

            urls.Add(url);
        }

        return urls;
    }
}