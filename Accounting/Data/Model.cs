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
    public static bool RegenerateDatabase { get; set; } = false;

    public static bool SeedAccounts { get; set; } = false;

    public static bool SeedVerifications { get; set; } = false;

    public static async void SeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AccountingContext>();

        if (RegenerateDatabase)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        if (SeedAccounts)
        {

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
                    Name = "Företagskonto",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 5010,
                    Name = "Lokalhyra",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 5220,
                    Name = "Hyra av inventarier och verktyg",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 5410,
                    Name = "Förbrukningsinventarier",
                    Description = "Inköp av inventarier som kostar mindre än ett halvt prisbasbelopp*, t.ex. dator, skrivare, kontorsstol"
                },
                new Account
                {
                    AccountNo = 5460,
                    Name = "Förbrukningsmaterial",
                    Description = "Material som förbrukas inom företaget, t.ex. spik, skruv"
                },
                new Account
                {
                    AccountNo = 5480,
                    Name = "Arbetskläder & skyddsmaterial",
                    Description = "Arbetskläder, Terminalglasögon Läs mer på Skatteverkets sida"
                },
                new Account
                {
                    AccountNo = 5910,
                    Name = "Annonsering",
                    Description = "Annonser jag köper inom Sverige"
                },
                new Account
                {
                    AccountNo = 5912,
                    Name = "Annonsering EU",
                    Description = "Annonser jag köper från företag in EU, t.ex. Google, Facebook när de fakturerar från Irland"
                },
                new Account
                {
                    AccountNo = 6212,
                    Name = "Mobiltelefon",
                    Description = "Telefonräkningen"
                },
                new Account
                {
                    AccountNo = 6540,
                    Name = "IT-tjänster",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 6541,
                    Name = "Redovisningsprogram",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 6570,
                    Name = "Banktjänster",
                    Description = "Alla tjänster och avgifter till din bank. Viktigt att tänka på att banktjänster är momsfria"
                },
                new Account
                {
                    AccountNo = 6910,
                    Name = " Licensavgifter och royalties",
                    Description = "Royalty och licensavgifter"
                },
                new Account
                {
                    AccountNo = 6970,
                    Name = "Tidningar, facklitteratur",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 1910,
                    Name = "Kassa",
                    Description = "Insättningar av kontanter från kassan"
                },
                new Account
                {
                    AccountNo = 1940,
                    Name = "Placeringskonto",
                    Description = "Överföringar till och från placeringskonto/sparkonto"
                },
                new Account
                {
                    AccountNo = 2013,
                    Name = "Egna Uttag",
                    Description = "Privata uttag \"lön\" i enskild firma"
                },
                new Account
                {
                    AccountNo = 2018,
                    Name = "Egna Insättningar",
                    Description = "Privata insättningar i enskild firma"
                },
                new Account
                {
                    AccountNo = 2650,
                    Name = "Redovisningskonto för moms",
                    Description = "Får tillbaka eller betalar moms till skatteverket"
                },
                new Account
                {
                    AccountNo = 3740,
                    Name = "Öresavrundning",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 8310,
                    Name = "Ränteintäkter",
                    Description = String.Empty
                },
                new Account
                {
                    AccountNo = 8410,
                    Name = "Räntekostnader",
                    Description = String.Empty
                });
        }

        if (SeedVerifications)
        {
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
        }

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