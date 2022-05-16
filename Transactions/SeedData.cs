using Transactions.Data;
using Transactions.Models;

using Microsoft.AspNetCore.Identity;

namespace Transactions;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            var context = scope.ServiceProvider.GetRequiredService<TransactionsContext>();
            //await context.Database.EnsureDeletedAsync();
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();
         
            //await context.SaveChangesAsync();
        }
    }
}