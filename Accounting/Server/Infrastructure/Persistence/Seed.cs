using Accounting.Domain.Entities;
using Accounting.Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure.Persistence;

public static class Seed
{
    public static bool Run { get; set; } = true;

    public static bool RecreateDatabase { get; set; } = true;

    public static bool SeedAccounts { get; set; } = true;

    public static bool SeedVerifications { get; set; } = true;

    static readonly int verificationId = 1;

    public static async Task SeedAsync(this IServiceProvider serviceProvider)
    {
        if (!Run)
        {
            return;
        }

        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AccountingContext>();

        if (RecreateDatabase)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        if (SeedAccounts)
        {
            DoSeedAccounts(context);
        }

        if (SeedVerifications)
        {
            DoSeedVerifications(context);
        }

        await context.SaveChangesAsync();
    }

    private static void DoSeedAccounts(AccountingContext context)
    {
        context.Accounts.AddRange(
            new Account
            {
                AccountNo = 1510,
                Class = AccountClass.Assets,
                Name = "Kundfordringar",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1930,
                Class = AccountClass.Assets,
                Name = "Företagskonto",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1910,
                Class = AccountClass.Assets,
                Name = "Kassa",
                Description = "Insättningar av kontanter från kassan"
            },
            new Account
            {
                AccountNo = 1920,
                Class = AccountClass.Assets,
                Name = "PlusGiro",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1630,
                Class = AccountClass.Assets,
                Name = "Skattekonto",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1940,
                Class = AccountClass.Assets,
                Name = "Placeringskonto",
                Description = "Överföringar till och från placeringskonto/sparkonto"
            },
            new Account
            {
                AccountNo = 2010,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Eget kapital",
                Description = String.Empty,
            },
            new Account
            {
                AccountNo = 2011,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Egna varuttag",
                Description = String.Empty,
            },
            new Account
            {
                AccountNo = 2013,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Egna Uttag",
                Description = "Privata uttag \"lön\" i enskild firma"
            },
            new Account
            {
                AccountNo = 2018,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Egna Insättningar",
                Description = "Privata insättningar i enskild firma"
            },
            new Account
            {
                AccountNo = 2068,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Vinst eller förlust från föregående år",
                Description = String.Empty,
            },
            new Account
            {
                AccountNo = 2330,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Checkkredit",
                Description = String.Empty,
            },
            new Account
            {
                AccountNo = 2411,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Kortfristiga lån från kreditinstitut",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2440,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Leverantörsskulder",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2455,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Kortfristiga lån i utländsk valuta",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2510,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Skatteskulder",
                Description = String.Empty,
            },
            new Account
            {
                AccountNo = 2610,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2611,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms Försäljning Sverige 25%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2620,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms 12%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2621,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms Försäljning Sverige 12%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2630,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms 6%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2631,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms Försäljning Sverige 6%",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2640,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Ingående moms",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 2650,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Redovisningskonto för moms",
                Description = "Får tillbaka eller betalar moms till skatteverket"
            },
            new Account
            {
                AccountNo = 2731,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Avräkning arbetsgivaravgift",
                Description = string.Empty
            },
            new Account
            {
                AccountNo = 3000,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3001,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning 25% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3002,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning 12% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3003,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning 6% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3040,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning tjänster",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3041,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning tjänster 25% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3042,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning tjänster 12% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3043,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning tjänster 6% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3050,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Varuförsäljning",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3051,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning varor 25% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3052,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning varor 12% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3053,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning varor 6% moms Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3231,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning med omvänd byggmoms",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3550,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Fakturerade resekostnader",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3740,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Öresavrundning",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3911,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Hyresintäkter",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3921,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Provisionsintäkter",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 3960,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Valutakursvinster av rörelsekaraktär",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 4000,
                Class = AccountClass.Costs,
                Name = "Inköp av varor från Sverige",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 4010,
                Class = AccountClass.Costs,
                Name = "Inköp av varor och material",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 5010,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Lokalhyra",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 5220,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Hyra av inventarier och verktyg",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 5410,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Förbrukningsinventarier",
                Description = "Inköp av inventarier som kostar mindre än ett halvt prisbasbelopp*, t.ex. dator, skrivare, kontorsstol"
            },
            new Account
            {
                AccountNo = 5460,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Förbrukningsmaterial",
                Description = "Material som förbrukas inom företaget, t.ex. spik, skruv"
            },
            new Account
            {
                AccountNo = 5480,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Arbetskläder & skyddsmaterial",
                Description = "Arbetskläder, Terminalglasögon Läs mer på Skatteverkets sida"
            },
            new Account
            {
                AccountNo = 5910,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Annonsering",
                Description = "Annonser jag köper inom Sverige"
            },
            new Account
            {
                AccountNo = 5912,
                Class = AccountClass.OtherOperatingExpenses1,
                Name = "Annonsering EU",
                Description = "Annonser jag köper från företag in EU, t.ex. Google, Facebook när de fakturerar från Irland"
            },
            new Account
            {
                AccountNo = 6212,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = "Mobiltelefon",
                Description = "Telefonräkningen"
            },
            new Account
            {
                AccountNo = 6540,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = "IT-tjänster",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 6541,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = "Redovisningsprogram",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 6570,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = "Banktjänster",
                Description = "Alla tjänster och avgifter till din bank. Viktigt att tänka på att banktjänster är momsfria"
            },
            new Account
            {
                AccountNo = 6910,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = " Licensavgifter och royalties",
                Description = "Royalty och licensavgifter"
            },
            new Account
            {
                AccountNo = 6970,
                Class = AccountClass.OtherOperatingExpenses2,
                Name = "Tidningar, facklitteratur",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7010,
                Class = AccountClass.PersonnelCosts,
                Name = "Löner till kollektivanställda",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7210,
                Class = AccountClass.PersonnelCosts,
                Name = "Löner till tjänstemän",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7220,
                Class = AccountClass.PersonnelCosts,
                Name = "Löner till företagsledare",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7332,
                Class = AccountClass.PersonnelCosts,
                Name = "Bilersättningar, skattepliktiga",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7510,
                Class = AccountClass.PersonnelCosts,
                Name = "Arbetsgivaravgifter",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7610,
                Class = AccountClass.PersonnelCosts,
                Name = "Utbildning",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7620,
                Class = AccountClass.PersonnelCosts,
                Name = "Sjuk- och hälsovård",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 7691,
                Class = AccountClass.PersonnelCosts,
                Name = "Personalrekrytering",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8014,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Koncernbidrag",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8310,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Ränteintäkter",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8311,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Ränteintäkter från bank",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8410,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Räntekostnader",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8415,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Räntekostnader kreditinstitut",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8420,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Räntekostnader för kortfristiga skulder",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8421,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Räntekostnader till kreditinstitut",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8990,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Resultat",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 8999,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Årets resultat",
                Description = String.Empty
            });
    }

    private static void DoSeedVerifications(AccountingContext context)
    {
        InsertMoney(context);

        YouSendAnInvoiceToCustomer(context);
        TheCustomerPaysTheInvoice(context);
        YouReceiveAInvoice(context);
        YouTransferFromPlusGiroToCorporateAccount(context);
        YouPayForTheInvoice(context);
        YouWithdrawMoneyAsSalary(context);

        //YouTransferFromTaxAccountToCorporateAccount(context);
    }

    private static void InsertMoney(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du sätter in egna pengar på företagskontot",
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
             new Entry
             {
                 AccountNo = 2018,
                 VerificationId = verificationId,
                 Description = string.Empty,
                 Credit = 30000m
             },
            new Entry
            {
                AccountNo = 1930,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 30000m
            }
        });
    }

    private static void YouSendAnInvoiceToCustomer(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du skickar en faktura"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 1510,
                Description = string.Empty,
                Debit = 10000m
            },
            new Entry
            {
                AccountNo = 2610,
                Description = string.Empty,
                Credit = 2000m
            },
            new Entry
            {
                AccountNo = 3001,
                Description = string.Empty,
                Credit = 8000m
            }
        });
    }

    private static void TheCustomerPaysTheInvoice(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Kunden betalar fakturan"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 1920,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 10000m
            },
            new Entry
            {
                AccountNo = 1510,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 10000m
            }
        });
    }

    private static void YouReceiveAInvoice(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du tar emot fakturan"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 4000,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 4000m
            },
            new Entry
            {
                AccountNo = 2640,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 1000m
            }, new Entry
            {
                AccountNo = 2440,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 5000m
            }
        });
    }

    private static void YouPayForTheInvoice(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du betalar fakturan"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 2440,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 5000m
            }, new Entry
            {
                AccountNo = 1930,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 5000m
            }
        });
    }


    private static void YouWithdrawMoneyAsSalary(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du tar ut egen lön"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 2013,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 30000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 30000m
            }
        });
    }

    private static void YouTransferFromPlusGiroToCorporateAccount(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du överför pengar från PlusGiro till företagskonto"
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 1920,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 10000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 10000m
            }
        });
    }

    private static void YouTransferFromTaxAccountToCorporateAccount(AccountingContext context)
    {
        var verification = new Verification
        {
            Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
            Description = "Du överför pengar från skattekonto till företagskonto",
        };

        context.Verifications.Add(verification);

        verification.Entries.AddRange(new[] {
            new Entry
            {
                AccountNo = 1630,
                VerificationId = verificationId,
                Description = string.Empty,
                Credit = 4000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationId = verificationId,
                Description = string.Empty,
                Debit = 4000m
            }
        });
    }
}