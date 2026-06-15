namespace Loomi.Transactions.Application.Interfaces;

public interface IClientApiService
{
    Task<bool> ClientExistsAsync(Guid clientId);
}