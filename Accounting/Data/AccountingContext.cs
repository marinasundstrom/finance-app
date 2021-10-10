using Microsoft.EntityFrameworkCore;

namespace Accounting.Data;

public class AccountingContext : DbContext
{
    public AccountingContext(DbContextOptions options) : base(options)
    {

    }

#nullable disable

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Verification> Verifications { get; set; }

    public DbSet<Entry> Entries { get; set; }

#nullable restore
}
