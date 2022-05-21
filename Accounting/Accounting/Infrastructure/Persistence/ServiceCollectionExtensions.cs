using Microsoft.EntityFrameworkCore;

using Accounting.Domain;
using Accounting.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Accounting.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddDbContext<AccountingContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("mssql", "Accounting"), o => o.EnableRetryOnFailure());
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IAccountingContext>(sp => sp.GetRequiredService<AccountingContext>());

        return services;
    }
}