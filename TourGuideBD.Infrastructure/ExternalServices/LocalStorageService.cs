using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Infrastructure.ExternalServices;

public class LocalStorageService : IBlobStorageService
{
    private readonly string _baseUploadPath;
    private readonly string _baseUrl;

    public LocalStorageService(IWebHostEnvironment env, IConfiguration configuration)
    {
        // Images folder: wwwroot/uploads/
        _baseUploadPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");
        _baseUrl = configuration["LocalStorage:BaseUrl"] ?? "https://localhost:44389";
    }

    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        // Folder বানাও: wwwroot/uploads/containerName/
        var folderPath = Path.Combine(_baseUploadPath, containerName);
        Directory.CreateDirectory(folderPath);

        // Unique filename
        var cleanFileName = Path.GetFileName(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}_{cleanFileName}";
        var filePath = Path.Combine(folderPath, uniqueFileName);

        // File save করো
        using var fileStreamOut = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fileStreamOut, cancellationToken);

        // Public URL return করো
        return $"{_baseUrl}/uploads/{containerName}/{uniqueFileName}";
    }

    public Task DeleteAsync(
        string fileUrl,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(fileUrl)) return Task.CompletedTask;

            // URL থেকে file path বের করো
            var uri = new Uri(fileUrl);
            var relativePath = uri.AbsolutePath.TrimStart('/');
            var filePath = Path.Combine(
                _baseUploadPath,
                relativePath.Replace("uploads/", "").Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Local delete warning: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}