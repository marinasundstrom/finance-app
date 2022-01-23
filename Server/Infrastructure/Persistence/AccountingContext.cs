using Accounting.Application.Common.Interfaces;
using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Infrastructure.Persistence;

public class AccountingContext : DbContext, IAccountingContext
{
    public AccountingContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Account).Assembly);
    }

#nullable disable

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Verification> Verifications { get; set; }

    public DbSet<Entry> Entries { get; set; }

#nullable restore
}
