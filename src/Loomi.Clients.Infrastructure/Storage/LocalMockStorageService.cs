using Loomi.Clients.Application.Interfaces;

namespace Loomi.Clients.Infrastructure.Storage;

public class LocalMockStorageService : IStorageService
{
    public Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
    {
        // Simula o retorno de uma URL do Azure Blob Storage
        var fakeUrl = $"https://loomistorage.blob.core.windows.net/profiles/{Guid.NewGuid()}-{fileName}";
        return Task.FromResult(fakeUrl);
    }
}