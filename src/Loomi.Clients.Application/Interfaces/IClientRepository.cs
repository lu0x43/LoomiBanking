using Loomi.Clients.Domain.Entities;

namespace Loomi.Clients.Application.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
}