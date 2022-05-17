using Microsoft.EntityFrameworkCore;

using Invoices.Models;

namespace Invoices.Data;

public interface IInvoicesContext
{
    DbSet<Invoice> Invoices { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
