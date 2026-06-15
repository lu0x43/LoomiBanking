using FluentAssertions;
using Loomi.Clients.Api.Controllers;
using Loomi.Clients.Application.DTOs;
using Loomi.Clients.Application.Interfaces;
using Loomi.Clients.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace Loomi.Clients.UnitTests.Api.Controllers;

public class ClientsControllerTests
{
    private readonly Mock<IDistributedCache> _cacheMock;

    private readonly ClientsController _controller;

    // cria os mocks
    private readonly Mock<IClientRepository> _repositoryMock;
    private readonly Mock<IStorageService> _storageServiceMock;

    public ClientsControllerTests()
    {
        _repositoryMock = new Mock<IClientRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _storageServiceMock = new Mock<IStorageService>();

        // Injeta no Controller real
        _controller = new ClientsController(_repositoryMock.Object, _cacheMock.Object, _storageServiceMock.Object);
    }

    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated201_And_CallRepository()
    {
        // Prepara uma requisição
        var request = new CreateClientRequest(
            "Lucas Caetano",
            "12345678900",
            "lucas@redteam.com",
            "033",
            "0001",
            "12345-6"
        );

        //Dispara no endpoint (sem passar pela rede HTTP, direto no método)
        var result = await _controller.Create(request);

        //Esperado = devolver um status 201 (CreatedAtAction)
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be("GetById");

        //Verificamos se o Controller realmente mandou o repositório salvar os dados
        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Client>()), Times.Once);
    }
}