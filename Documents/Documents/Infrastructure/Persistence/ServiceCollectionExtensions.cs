using Microsoft.EntityFrameworkCore;

using Documents.Domain;

namespace Documents.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) 
    {
        const string ConnectionStringKey = "mssql";

        var connectionString = Documents.ConfigurationExtensions.GetConnectionString(configuration, ConnectionStringKey, "Documents");

        services.AddDbContext<DocumentsContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
            options.EnableSensitiveDataLogging();
#endif
        });

        services.AddScoped<IDocumentsContext>(sp => sp.GetRequiredService<DocumentsContext>());

        return services;
    }
}