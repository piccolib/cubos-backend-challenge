using CubosFinance.Application.DTOs.Common;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using CubosFinance.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CubosFinance.Tests.Services;

public class CardServiceTests
{
    private readonly Mock<ICardRepository> _cardRepositoryMock = new();
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly CardService _service;

    public CardServiceTests()
    {
        _service = new CardService(
            _cardRepositoryMock.Object,
            _accountRepositoryMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidCvv_ThrowsInvalidCvvException()
    {
        var dto = new CreateCardDto
        {
            Type = CardType.Virtual,
            Number = "1234567890123456",
            Cvv = "12" // inválido
        };

        await Assert.ThrowsAsync<InvalidCvvException>(() => _service.CreateAsync(Guid.NewGuid(), dto));
    }

    [Fact]
    public async Task CreateAsync_WithExistingPhysicalCard_ThrowsException()
    {
        var dto = new CreateCardDto
        {
            Type = CardType.Physical,
            Number = "1234567890123456",
            Cvv = "123"
        };

        _cardRepositoryMock.Setup(r => r.PhysicalCardExistsAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(true);

        await Assert.ThrowsAsync<PhysicalCardAlreadyExistsException>(() => _service.CreateAsync(Guid.NewGuid(), dto));
    }

    [Fact]
    public async Task CreateAsync_WithValidVirtualCard_ReturnsResponseDto()
    {
        var dto = new CreateCardDto
        {
            Type = CardType.Virtual,
            Number = "1234567890123456",
            Cvv = "123"
        };

        _cardRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Card>()))
                           .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(Guid.NewGuid(), dto);

        Assert.NotNull(result);
        Assert.Equal(CardType.Virtual, result.Type);
        Assert.Equal("3456", result.Number);
        Assert.Equal("123", result.Cvv);
    }

    [Fact]
    public async Task GetAllByAccountAsync_ReturnsCards()
    {
        var accountId = Guid.NewGuid();

        var fakeCards = new List<Card>
        {
            new Card
            {
                Id = Guid.NewGuid(),
                Type = CardType.Physical,
                Number = "1234567890123456",
                Cvv = "111"
            }
        };

        _cardRepositoryMock.Setup(r => r.GetAllByAccountIdAsync(accountId))
                           .ReturnsAsync(fakeCards);

        var result = await _service.GetAllByAccountAsync(accountId);

        var cardList = result.ToList();
        Assert.Single(cardList);
        Assert.Equal("1234567890123456", cardList[0].Number);
    }

    [Fact]
    public async Task GetAllByPersonAsync_ReturnsPagedResponse()
    {
        var personId = Guid.NewGuid();

        var fakeCards = new List<Card>
        {
            new Card
            {
                Id = Guid.NewGuid(),
                Type = CardType.Virtual,
                Number = "9876543210987654",
                Cvv = "222"
            }
        };

        _cardRepositoryMock.Setup(r => r.GetByPersonIdAsync(personId, 1, 10))
                           .ReturnsAsync(fakeCards);

        var result = await _service.GetAllByPersonAsync(personId, 1, 10);

        Assert.Single(result.Data);
        Assert.Equal("7654", result.Data.First().Number);
        Assert.Equal(1, result.Pagination.CurrentPage);
        Assert.Equal(10, result.Pagination.ItemsPerPage);
    }
}
