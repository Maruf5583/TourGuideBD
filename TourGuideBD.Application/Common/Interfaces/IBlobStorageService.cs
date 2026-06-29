namespace TourGuideBD.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string containerName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string fileUrl,
        string containerName,
        CancellationToken cancellationToken = default);
}