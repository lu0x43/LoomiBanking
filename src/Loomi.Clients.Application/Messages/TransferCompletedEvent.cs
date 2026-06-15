namespace Loomi.Transactions.Application.Messages;
// O namespace tem que ser o de Transactions intencionalmente

public record TransferCompletedEvent(
    Guid TransactionId,
    Guid FromClientId,
    Guid ToClientId,
    decimal Amount,
    DateTime CompletedAt);