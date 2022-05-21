using Invoices.Domain;
using Invoices.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Infrastructure.Persistence;

public class InvoicesContext : DbContext, IInvoicesContext
{
    public InvoicesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; } = null!;
}