using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Integrations.Complience;
using CubosFinance.Application.Integrations.Complience.Settings;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Persistence.Repositories;
using Refit;

namespace CubosFinance.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<IPersonRepository, PersonRepository>();

        // Application Services
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IComplianceValidationService, ComplianceValidationService>();
        services.AddScoped<IComplianceAuthService, ComplianceAuthService>();

        services
            .AddRefitClient<IComplianceApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(configuration["ComplianceApi:BaseUrl"]!);
            });

        services.Configure<ComplianceAuthSettings>(configuration.GetSection("ComplianceAuth"));

        return services;
    }
}
