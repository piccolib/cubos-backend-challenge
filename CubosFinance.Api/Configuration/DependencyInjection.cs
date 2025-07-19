using CubosFinance.Application.Interfaces;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Persistence.Repositories;

namespace CubosFinance.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IPersonRepository, PersonRepository>();

        // Application Services
        services.AddScoped<IPersonService, PersonService>();

        return services;
    }
}
