using Invoices.Models;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Data;

public interface IInvoicesContext
{
    DbSet<Invoice> Invoices { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}