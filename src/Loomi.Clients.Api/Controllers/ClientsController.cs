using Loomi.Clients.Application.DTOs;
using Loomi.Clients.Application.Interfaces;
using Loomi.Clients.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Loomi.Clients.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IStorageService _storageService;

    public ClientsController(IClientRepository repository, IMemoryCache cache, IStorageService storageService)
    {
        _repository = repository;
        _cache = cache;
        _storageService = storageService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        var bankingDetails = new BankingDetails(request.BankCode, request.Branch, request.AccountNumber);
        var client = new Client(request.FullName, request.Document, request.Email, bankingDetails);
        
        await _repository.AddAsync(client);
        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client.Id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cacheKey = $"Client_{id}";
        
        if (!_cache.TryGetValue(cacheKey, out ClientResponse? response))
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null) return NotFound();

            response = new ClientResponse(client.Id, client.FullName, client.Document, client.Email, client.ProfilePictureUrl, 
                client.BankingDetails.BankCode, client.BankingDetails.Branch, client.BankingDetails.AccountNumber, client.CreatedAt);
            
            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
        }
        
        return Ok(response);
    }

    [HttpPatch("{id}/photo")]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Arquivo inválido.");

        var client = await _repository.GetByIdAsync(id);
        if (client == null) return NotFound();

        using var stream = file.OpenReadStream();
        var url = await _storageService.UploadFileAsync(file.FileName, stream, file.ContentType);
        
        client.UpdateProfilePicture(url);
        await _repository.UpdateAsync(client);
        
        _cache.Remove($"Client_{id}");

        return Ok(new { ProfilePictureUrl = url });
    }
}