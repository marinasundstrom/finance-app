using Transactions.Application.Common.Interfaces;
using Transactions.Infrastructure.Services;
using Transactions.Infrastructure.Persistence;

namespace Transactions.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddPersistence(configuration);

        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTime, DateTimeService>();
        
        return services;
    }
}