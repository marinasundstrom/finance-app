using System;

using Invoices.Models;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Data;

public class InvoicesContext : DbContext, IInvoicesContext
{
    public InvoicesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; } = null!;
}