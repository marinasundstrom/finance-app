using Invoices.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Domain;

public interface IInvoicesContext
{
    DbSet<Invoice> Invoices { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}