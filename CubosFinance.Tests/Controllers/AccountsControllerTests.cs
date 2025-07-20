using CubosFinance.Api.Controllers;
using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Account;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace CubosFinance.Tests.Controllers;

public class AccountsControllerTests
{
    private readonly Mock<IAccountService> _accountServiceMock = new();
    private readonly Mock<ICardService> _cardServiceMock = new();
    private readonly Mock<ITransactionService> _transactionServiceMock = new();

    private readonly AccountsController _controller;

    public AccountsControllerTests()
    {
        _controller = new AccountsController(
            _accountServiceMock.Object,
            _cardServiceMock.Object,
            _transactionServiceMock.Object
        );
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenAccountCreated()
    {
        var dto = new CreateAccountDto { Branch = "123", Account = "1234567-8" };
        var response = new AccountResponseDto { Id = Guid.NewGuid(), Branch = "123", Account = "1234567-8" };

        _accountServiceMock.Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(response);

        var result = await _controller.Create(dto) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(response, result.Value);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenBranchInvalid()
    {
        var dto = new CreateAccountDto { Branch = "12", Account = "1234567-8" };
        _accountServiceMock.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new InvalidBranchFormatException());

        await Assert.ThrowsAsync<InvalidBranchFormatException>(() => _controller.Create(dto));
    }

    [Fact]
    public async Task Create_ShouldThrowDuplicatedAccountNumberException_WhenAccountNumberExists()
    {
        var dto = new CreateAccountDto { Branch = "123", Account = "1234567-8" };

        _accountServiceMock
            .Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new DuplicatedAccountNumberException());

        await Assert.ThrowsAsync<DuplicatedAccountNumberException>(() => _controller.Create(dto));
    }

    [Fact]
    public async Task Get_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
    {
        var controller = new AccountsController(_accountServiceMock.Object, _cardServiceMock.Object, _transactionServiceMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await controller.Get() as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenUserIsAuthenticated()
    {
        var personId = Guid.NewGuid();
        var fakeAccounts = new List<AccountResponseDto>
        {
            new AccountResponseDto { Id = Guid.NewGuid(), Branch = "123", Account = "1234567-8" }
        };

        _accountServiceMock.Setup(s => s.GetAllByUserAsync(personId))
            .ReturnsAsync(fakeAccounts);

        var controller = new AccountsController(_accountServiceMock.Object, _cardServiceMock.Object, _transactionServiceMock.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, personId.ToString())
                }))
            }
        };

        var result = await controller.Get() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(fakeAccounts, result.Value);
    }
}