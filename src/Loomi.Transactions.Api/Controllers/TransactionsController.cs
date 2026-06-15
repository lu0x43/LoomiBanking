using Loomi.Transactions.Application.DTOs;
using Loomi.Transactions.Application.Interfaces;
using Loomi.Transactions.Domain.Entities;
using Loomi.Transactions.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Loomi.Transactions.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionsDbContext _context;
    private readonly IClientApiService _clientService;

    public TransactionsController(TransactionsDbContext context, IClientApiService clientService)
    {
        _context = context;
        _clientService = clientService;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] CreateTransactionRequest request)
    {
        var fromExists = await _clientService.ClientExistsAsync(request.FromClientId);
        var toExists = await _clientService.ClientExistsAsync(request.ToClientId);

        if (!fromExists || !toExists) 
            return BadRequest("Conta de origem ou destino não encontrada no MS de Clientes.");

        var transaction = new Transaction(request.FromClientId, request.ToClientId, request.Amount);
        
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(new { transaction.Id, Status = "Completed" });
    }
}