using Microsoft.EntityFrameworkCore;

using Payments.Domain;

namespace Payments.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        const string ConnectionStringKey = "mssql";

        var connectionString = configuration.GetConnectionString(ConnectionStringKey, "Payments");

        services.AddDbContext<PaymentsContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IPaymentsContext>(sp => sp.GetRequiredService<PaymentsContext>());

        return services;
    }
}