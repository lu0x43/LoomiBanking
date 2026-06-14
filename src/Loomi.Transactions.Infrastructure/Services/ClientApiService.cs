using Loomi.Transactions.Application.Interfaces;

namespace Loomi.Transactions.Infrastructure.Services;

public class ClientApiService : IClientApiService
{
    private readonly HttpClient _httpClient;

    public ClientApiService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<bool> ClientExistsAsync(Guid clientId)
    {
        var response = await _httpClient.GetAsync($"/api/v1/clients/{clientId}");
        return response.IsSuccessStatusCode;
    }
}