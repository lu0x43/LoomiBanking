namespace Loomi.Transactions.Application.DTOs;

public record CreateTransactionRequest(Guid FromClientId, Guid ToClientId, decimal Amount);