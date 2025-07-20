using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace CubosFinance.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _service = new AccountService(_accountRepositoryMock.Object, _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsAccountResponse()
    {
        // Arrange
        var dto = new CreateAccountDto
        {
            Branch = "123",
            Account = "1234567-8"
        };

        var userId = Guid.NewGuid();
        SetupHttpContext(userId);
        _accountRepositoryMock.Setup(r => r.ExistsByNumberAsync(dto.Account)).ReturnsAsync(false);
        _accountRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal(dto.Branch, result.Branch);
        Assert.Equal(dto.Account, result.Account);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task CreateAsync_InvalidBranch_ThrowsInvalidBranchFormatException()
    {
        var dto = new CreateAccountDto { Branch = "12", Account = "1234567-8" };
        await Assert.ThrowsAsync<InvalidBranchFormatException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_InvalidAccount_ThrowsInvalidAccountFormatException()
    {
        var dto = new CreateAccountDto { Branch = "123", Account = "1234" };
        await Assert.ThrowsAsync<InvalidAccountFormatException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ExistingAccount_ThrowsDuplicatedAccountNumberException()
    {
        var dto = new CreateAccountDto { Branch = "123", Account = "1234567-8" };
        _accountRepositoryMock.Setup(r => r.ExistsByNumberAsync(dto.Account)).ReturnsAsync(true);
        await Assert.ThrowsAsync<DuplicatedAccountNumberException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_UnauthenticatedUser_ThrowsUnauthorizedAccessException()
    {
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);
        var dto = new CreateAccountDto { Branch = "123", Account = "1234567-8" };
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task GetAllByUserAsync_ReturnsMappedAccounts()
    {
        var personId = Guid.NewGuid();
        var accounts = new List<Account>
        {
            new Account("123", "1111111-1", personId),
            new Account("456", "2222222-2", personId)
        };

        _accountRepositoryMock.Setup(r => r.GetByPersonIdAsync(personId)).ReturnsAsync(accounts);

        var result = await _service.GetAllByUserAsync(personId);

        Assert.Collection(result,
            item => Assert.Equal("1111111-1", item.Account),
            item => Assert.Equal("2222222-2", item.Account));
    }

    private void SetupHttpContext(Guid userId)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        });

        var user = new ClaimsPrincipal(identity);

        var contextMock = new DefaultHttpContext { User = user };

        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(contextMock);
    }
}
