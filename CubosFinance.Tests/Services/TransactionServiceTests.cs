using CubosFinance.Application.DTOs.Transactions;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using Moq;

namespace CubosFinance.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new();
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new();

    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _service = new TransactionService(
            _transactionRepositoryMock.Object,
            _accountRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidCreditTransaction_ReturnsResponse()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var dto = new CreateTransactionDto { Value = 100, Description = "Crédito" };

        _accountRepositoryMock.Setup(r => r.ExistsAsync(accountId)).ReturnsAsync(true);
        _transactionRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Transaction>()))
                                  .ReturnsAsync((Transaction t) => t);

        // Act
        var result = await _service.CreateAsync(accountId, dto);

        // Assert
        Assert.Equal(dto.Value, result.Value);
        Assert.Equal(dto.Description, result.Description);
    }

    [Fact]
    public async Task CreateAsync_WithInsufficientBalance_ThrowsException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var dto = new CreateTransactionDto { Value = -200, Description = "Débito" };

        _accountRepositoryMock.Setup(r => r.ExistsAsync(accountId)).ReturnsAsync(true);
        _transactionRepositoryMock.Setup(r => r.GetCurrentBalanceAsync(accountId)).ReturnsAsync(100);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.CreateAsync(accountId, dto));
    }

    [Fact]
    public async Task CreateInternalAsync_WithSameSenderAndReceiver_ThrowsException()
    {
        var id = Guid.NewGuid();
        var dto = new CreateInternalTransferDto
        {
            ReceiverAccountId = id,
            Value = 50,
            Description = "Transferência"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateInternalAsync(id, dto));
    }

    [Fact]
    public async Task CreateInternalAsync_WithInsufficientBalance_ThrowsException()
    {
        var senderId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        var dto = new CreateInternalTransferDto
        {
            ReceiverAccountId = receiverId,
            Value = 100,
            Description = "Transferência"
        };

        _accountRepositoryMock.Setup(r => r.ExistsAsync(senderId)).ReturnsAsync(true);
        _accountRepositoryMock.Setup(r => r.ExistsAsync(receiverId)).ReturnsAsync(true);
        _transactionRepositoryMock.Setup(r => r.GetCurrentBalanceAsync(senderId)).ReturnsAsync(50);

        await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.CreateInternalAsync(senderId, dto));
    }

    [Fact]
    public async Task GetBalanceByAccountAsync_WithValidAccount_ReturnsBalance()
    {
        var accountId = Guid.NewGuid();

        _accountRepositoryMock.Setup(r => r.ExistsAsync(accountId)).ReturnsAsync(true);
        _transactionRepositoryMock.Setup(r => r.GetCurrentBalanceAsync(accountId)).ReturnsAsync(250);

        var result = await _service.GetBalanceByAccountAsync(accountId);

        Assert.Equal(250, result.Balance);
    }

    [Fact]
    public async Task RevertAsync_WithAlreadyReversed_ThrowsException()
    {
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        _transactionRepositoryMock.Setup(r => r.GetByIdAsync(transactionId))
                                  .ReturnsAsync(new Transaction { Id = transactionId, AccountId = accountId, Value = 100 });

        _transactionRepositoryMock.Setup(r => r.ExistsReversalFor(transactionId)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RevertAsync(accountId, transactionId));
    }

    [Fact]
    public async Task RevertAsync_WithInsufficientBalance_ThrowsException()
    {
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();

        _transactionRepositoryMock.Setup(r => r.GetByIdAsync(transactionId))
                                  .ReturnsAsync(new Transaction { Id = transactionId, AccountId = accountId, Value = 100 });

        _transactionRepositoryMock.Setup(r => r.ExistsReversalFor(transactionId)).ReturnsAsync(false);
        _transactionRepositoryMock.Setup(r => r.GetCurrentBalanceAsync(accountId)).ReturnsAsync(50);

        await Assert.ThrowsAsync<InsufficientBalanceException>(() => _service.RevertAsync(accountId, transactionId));
    }

    [Fact]
    public async Task GetAllByAccountAsync_ReturnsPagedResult()
    {
        var accountId = Guid.NewGuid();
        var request = new TransactionResquestDto
        {
            CurrentPage = 1,
            ItemsPerPage = 5,
            Type = null
        };

        var fakeTransactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Value = 100, Description = "Teste" }
        };

        _transactionRepositoryMock.Setup(r => r.GetByAccountIdAsync(accountId, 1, 5, null))
                                  .ReturnsAsync(fakeTransactions);

        var result = await _service.GetAllByAccountAsync(accountId, request);

        Assert.Single(result.Data);
        Assert.Equal(1, result.Pagination.CurrentPage);
    }
}