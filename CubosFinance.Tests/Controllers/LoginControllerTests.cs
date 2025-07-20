using CubosFinance.API.Controllers;
using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Login;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Authentication;
using Xunit;

namespace CubosFinance.Tests.Controllers;

public class LoginControllerTests
{
    private readonly Mock<ILoginService> _loginServiceMock;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _loginServiceMock = new Mock<ILoginService>();
        _controller = new LoginController(_loginServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsInvalid()
    {
        var dto = new LoginRequestDto { Document = "12345678900", Password = "wrongpass" };

        _loginServiceMock.Setup(s => s.LoginAsync(dto))
            .ThrowsAsync(new InvalidCredentialException());

        await Assert.ThrowsAsync<InvalidCredentialException>(() => _controller.Login(dto));
    }
}