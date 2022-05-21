using Invoices.Infrastructure.Persistence;

namespace Invoices.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddPersistence(configuration);
        return services;
    }
}