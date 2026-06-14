using Loomi.Transactions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loomi.Transactions.Infrastructure.Data;

public class TransactionsDbContext : DbContext
{
    public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }
}