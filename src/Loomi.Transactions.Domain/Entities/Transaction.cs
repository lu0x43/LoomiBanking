namespace Loomi.Transactions.Domain.Entities;

public class Transaction
{
    protected Transaction()
    {
    }

    public Transaction(Guid fromClientId, Guid toClientId, decimal amount)
    {
        Id = Guid.NewGuid();
        FromClientId = fromClientId;
        ToClientId = toClientId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid FromClientId { get; private set; }
    public Guid ToClientId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
}