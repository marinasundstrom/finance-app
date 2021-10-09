using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

public static class AccountingContextSeed
{
    public static async void SeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AccountingContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Accounts.AddRange(
            new Account
            {
                AccountNo = 1510,
                Name = "Kundfordringar",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3051,
                Name = "Försäljning varor 25% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3041,
                Name = "Försäljning tjänster 25% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2611,
                Name = "Utgående moms Försäljning Sverige 25%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1930,
                Name = "Bank",
                Description = String.Empty
            });

        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = "V1",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = string.Empty,
                Attachment = String.Empty
            },
            new Verification
            {
                VerificationNo = "V2",
                Date = DateTime.Now,
                Description = string.Empty,
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 1510,
                VerificationNo = "V1",
                Description = string.Empty,
                Debit = 6500m
            },
            new Entry
            {
                AccountNo = 3051,
                VerificationNo = "V1",
                Description = string.Empty,
                Credit = 4800m
            },
            new Entry
            {
                AccountNo = 3041,
                VerificationNo = "V1",
                Description = string.Empty,
                Credit = 400m
            },
            new Entry
            {
                AccountNo = 2611,
                VerificationNo = "V1",
                Description = string.Empty,
                Credit = 1300m
            });

        context.Entries.AddRange(
           new Entry
           {
               AccountNo = 1510,
               VerificationNo = "V2",
               Description = string.Empty,
               Credit = 6500m
           },
           new Entry
           {
               AccountNo = 1930,
               VerificationNo = "V2",
               Description = string.Empty,
               Debit = 6500m
           });

        await context.SaveChangesAsync();
    }
}

public class Account
{
    [Key]
    public int AccountNo { get; set; }

    public int Class { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<Entry> Entries { get; set; } = new List<Entry>();
}

public class Verification
{
    [Key]
    public string VerificationNo { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public string Attachment { get; set; } = null!;

    public List<Entry> Entries { get; set; } = new List<Entry>();
}

public class Entry
{
    [Key]
    public int Id { get; set; }

    public string VerificationNo { get; set; } = null!;

    [ForeignKey(nameof(VerificationNo))]
    public Verification Verification { get; set; } = null!;

    public int AccountNo { get; set; }

    [ForeignKey(nameof(AccountNo))]
    public Account Account { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }
}