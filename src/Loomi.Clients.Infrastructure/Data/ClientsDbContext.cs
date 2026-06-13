using Loomi.Clients.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loomi.Clients.Infrastructure.Data;

public class ClientsDbContext : DbContext
{
    public ClientsDbContext(DbContextOptions<ClientsDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.FullName).IsRequired().HasMaxLength(150);
            entity.Property(c => c.Document).IsRequired().HasMaxLength(20);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            
            // Mapeando o Value Object para a mesma tabela
            entity.OwnsOne(c => c.BankingDetails, b =>
            {
                b.Property(p => p.BankCode).HasColumnName("BankCode").HasMaxLength(10).IsRequired();
                b.Property(p => p.Branch).HasColumnName("Branch").HasMaxLength(10).IsRequired();
                b.Property(p => p.AccountNumber).HasColumnName("AccountNumber").HasMaxLength(20).IsRequired();
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}