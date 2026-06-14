namespace Loomi.Clients.Application.DTOs;

public record CreateClientRequest(string FullName, string Document, string Email, string BankCode, string Branch, string AccountNumber);

public record ClientResponse(Guid Id, string FullName, string Document, string Email, string? ProfilePictureUrl, string BankCode, string Branch, string AccountNumber, DateTime CreatedAt);