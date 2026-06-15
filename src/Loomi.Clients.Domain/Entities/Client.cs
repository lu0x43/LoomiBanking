namespace Loomi.Clients.Domain.Entities;

public class Client
{
    protected Client()
    {
    } // Construtor obrigatório para o EF Core

    public Client(string fullName, string document, string email, BankingDetails bankingDetails)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Document = document;
        Email = email;
        BankingDetails = bankingDetails;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public BankingDetails BankingDetails { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void UpdateProfilePicture(string url)
    {
        ProfilePictureUrl = url;
    }
}

// O Value Object que será incorporado na tabela do Cliente
public class BankingDetails
{
    public BankingDetails(string bankCode, string branch, string accountNumber)
    {
        BankCode = bankCode;
        Branch = branch;
        AccountNumber = accountNumber;
    }

    public string BankCode { get; private set; }
    public string Branch { get; private set; }
    public string AccountNumber { get; private set; }
}