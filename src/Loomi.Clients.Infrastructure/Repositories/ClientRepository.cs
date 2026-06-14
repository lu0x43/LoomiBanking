using Loomi.Clients.Application.Interfaces;
using Loomi.Clients.Domain.Entities;
using Loomi.Clients.Infrastructure.Data;

namespace Loomi.Clients.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ClientsDbContext _context;

    public ClientRepository(ClientsDbContext context) => _context = context;

    public async Task<Client?> GetByIdAsync(Guid id) => await _context.Clients.FindAsync(id);

    public async Task AddAsync(Client client) 
    { 
        _context.Clients.Add(client); 
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateAsync(Client client) 
    { 
        _context.Clients.Update(client); 
        await _context.SaveChangesAsync(); 
    }
}