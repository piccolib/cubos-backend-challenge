using CubosFinance.Application.DTOs;
using CubosFinance.Application.Integrations.Complience;
using CubosFinance.Application.Integrations.Complience.Settings;
using CubosFinance.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Refit;

namespace CubosFinance.Tests.Integration.Compliance;

public class ComplianceIntegrationTests
{
    private readonly IComplianceApi _api;
    private readonly ComplianceAuthService _authService;
    private readonly ComplianceValidationService _validationService;

    public ComplianceIntegrationTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<ComplianceIntegrationTests>()
            .Build();

        var settings = configuration
            .GetSection("ComplianceAuth")
            .Get<ComplianceAuthSettings>();

        if (settings is null)
            throw new InvalidOperationException("ComplianceAuth not found in configuration.");

        var options = Options.Create(settings);
        _api = RestService.For<IComplianceApi>(settings.BaseUrl);
        _authService = new ComplianceAuthService(_api, options);
        _validationService = new ComplianceValidationService(_api, _authService);
    }

    [Fact(DisplayName = "Should obtain access token from Compliance API")]
    public async Task Should_Get_AccessToken()
    {
        var token = await _authService.GetAccessTokenAsync();

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Theory(DisplayName = "Should validate document successfully")]
    [InlineData("518.581.213-37")]
   // [InlineData("70.408.530/0001-59")]
    public async Task Should_Validate_Document(string document)
    {
        var dto = new DocumentValidationRequestDto
        {
            Document = document
        };

        var result = await _validationService.ValidateAsync(dto);

        Assert.True(result);
    }
}