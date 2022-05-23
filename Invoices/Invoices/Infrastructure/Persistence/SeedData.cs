using Invoices.Domain.Entities;
using Invoices.Domain.Enums;

namespace Invoices.Infrastructure.Persistence;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            var context = scope.ServiceProvider.GetRequiredService<InvoicesContext>();
            await context.Database.EnsureDeletedAsync();
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();

            if (!context.Invoices.Any())
            {
                var invoice1 = new Invoice(DateTime.Now.Subtract(TimeSpan.FromDays(-10)), InvoiceStatus.Draft, 75, 25, null, 100);
                invoice1.AddItem(ProductType.Good, "Item 1", 30, 0.25, 2);

                context.Invoices.Add(invoice1);

                var invoice2 = new Invoice(DateTime.Now, InvoiceStatus.Draft, 750, 250, null, 1000);
                invoice2.AddItem(ProductType.Good, "Item 1", 20, 0.25, 2);
                invoice2.AddItem(ProductType.Good, "Item 2", 100, 0.25, 3);

                context.Invoices.Add(invoice2);

                var invoice3 = new Invoice(DateTime.Now, InvoiceStatus.Draft, 750, 250, null, 1000);
                invoice3.AddItem(ProductType.Service, "Konsultarbete", 890, 0.25, 40);

                context.Invoices.Add(invoice3);

                await context.SaveChangesAsync();
            }
        }
    }
}