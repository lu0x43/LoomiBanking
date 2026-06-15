namespace Loomi.Transactions.Application.Messages;

public record TransferCompletedEvent(Guid TransactionId, Guid FromClientId, Guid ToClientId, decimal Amount, DateTime CompletedAt);