namespace Loomi.Clients.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType);
}