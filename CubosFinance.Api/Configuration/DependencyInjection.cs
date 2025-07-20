using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Integrations.Complience;
using CubosFinance.Application.Integrations.Complience.Services;
using CubosFinance.Application.Integrations.Complience.Settings;
using CubosFinance.Application.Services;
using CubosFinance.Application.Settings;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Persistence.Repositories;
using Refit;

namespace CubosFinance.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IComplianceValidationService, ComplianceValidationService>();
        services.AddScoped<IComplianceAuthService, ComplianceAuthService>();

        services
            .AddRefitClient<IComplianceApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["ComplianceAuth:BaseUrl"]!);
            });

        services.Configure<ComplianceAuthSettings>(configuration.GetSection("ComplianceAuth"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        return services;
    }
}
