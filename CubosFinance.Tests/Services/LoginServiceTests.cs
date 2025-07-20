using CubosFinance.Application.DTOs.Login;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using CubosFinance.Application.Settings;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace CubosFinance.Tests.Services;

public class LoginServiceTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly IOptions<JwtSettings> _jwtOptions;
    private readonly JwtSettings _jwtSettings;

    private readonly LoginService _service;

    public LoginServiceTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _jwtSettings = new JwtSettings
        {
            Key = "ThisIsASecretKeyForTestingOnly!#$",
            ExpiresInMinutes = 60,
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };

        _jwtOptions = Options.Create(_jwtSettings);

        _service = new LoginService(
            _repositoryMock.Object,
            _configurationMock.Object,
            _jwtOptions);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Document = "12345678900",
            Password = "password123"
        };

        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Bruno",
            Document = dto.Document,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _repositoryMock
            .Setup(r => r.GetByDocumentAsync(dto.Document))
            .ReturnsAsync(person);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.StartsWith("Bearer ", result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Document = "12345678900",
            Password = "wrongpassword"
        };

        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Bruno",
            Document = dto.Document,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
        };

        _repositoryMock
            .Setup(r => r.GetByDocumentAsync(dto.Document))
            .ReturnsAsync(person);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _service.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_WhenPersonNotFound_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Document = "00000000000",
            Password = "any"
        };

        _repositoryMock
            .Setup(r => r.GetByDocumentAsync(dto.Document))
            .ReturnsAsync((Person?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _service.LoginAsync(dto));
    }
}
