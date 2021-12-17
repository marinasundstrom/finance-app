using Microsoft.EntityFrameworkCore;

namespace Accounting.Server.Data;

public class AccountingContext : DbContext
{
    public AccountingContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Account>()
            .HasKey(x => x.AccountNo);

        modelBuilder
            .Entity<Account>()
            .Property(x => x.AccountNo)
            .ValueGeneratedNever();
    }

#nullable disable

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Verification> Verifications { get; set; }

    public DbSet<Entry> Entries { get; set; }

#nullable restore
}
