namespace Accounting.Data;

public static class Seed
{
    public static bool RecreateDatabase { get; set; } = true;

    public static bool SeedAccounts { get; set; } = true;

    public static bool SeedVerifications { get; set; } = true;

    static int verificationNo = 1;

    public static async void SeedAsync(this IServiceProvider serviceProvider)
    {
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
                AccountNo = 2640,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Ingående moms",
                Description = String.Empty
            },
            new Account
            {
                AccountNo = 1510,
                Class = AccountClass.Assets,
                Name = "Kundfordringar",
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
                AccountNo = 3051,
                Class = AccountClass.OperatingIncomeRevenue,
                Name = "Försäljning varor 25% moms Sverige",
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
                AccountNo = 2610,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Utgående moms 25%",
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
                AccountNo = 1930,
                Class = AccountClass.Assets,
                Name = "Företagskonto",
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
                AccountNo = 1910,
                Class = AccountClass.Assets,
                Name = "Kassa",
                Description = "Insättningar av kontanter från kassan"
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
                AccountNo = 2650,
                Class = AccountClass.EquityAndLiabilites,
                Name = "Redovisningskonto för moms",
                Description = "Får tillbaka eller betalar moms till skatteverket"
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
                AccountNo = 8310,
                Class = AccountClass.FinancialAndOtherIncomeAndExpenses,
                Name = "Ränteintäkter",
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
                AccountNo = 1920,
                Class = AccountClass.Assets,
                Name = "PlusGiro",
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
                AccountNo = 1630,
                Class = AccountClass.Assets,
                Name = "Skattekonto",
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
        context.Verifications.AddRange(
            new Verification
            {

                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du sätter in egna pengar på företagskontot",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
             new Entry
             {
                 AccountNo = 2018,
                 VerificationNo = $"V{verificationNo}",
                 Description = string.Empty,
                 Credit = 30000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 30000m
            });

        verificationNo++;
    }

    private static void YouSendAnInvoiceToCustomer(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {

                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du skickar en faktura",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 1510,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 10000m
            },
            new Entry
            {
                AccountNo = 2610,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 2000m
            },
            new Entry
            {
                AccountNo = 3001,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 8000m
            });

        verificationNo++;
    }

    private static void TheCustomerPaysTheInvoice(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Kunden betalar fakturan",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 1920,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 10000m
            },
            new Entry
            {
                AccountNo = 1510,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 10000m
            });

        verificationNo++;
    }

    private static void YouReceiveAInvoice(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du tar emot fakturan",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 4000,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 4000m
            },
            new Entry
            {
                AccountNo = 2640,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 1000m
            }, new Entry
            {
                AccountNo = 2440,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 5000m
            });

        verificationNo++;
    }

    private static void YouPayForTheInvoice(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du betalar fakturan",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 2440,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 5000m
            }, new Entry
            {
                AccountNo = 1930,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 5000m
            });

        verificationNo++;
    }


    private static void YouWithdrawMoneyAsSalary(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du tar ut egen lön",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 2013,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 30000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 30000m
            });

        verificationNo++;
    }

    private static void YouTransferFromPlusGiroToCorporateAccount(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du överför pengar från PlusGiro till företagskonto",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 1920,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 10000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 10000m
            });

        verificationNo++;
    }

    private static void YouTransferFromTaxAccountToCorporateAccount(AccountingContext context)
    {
        context.Verifications.AddRange(
            new Verification
            {
                VerificationNo = $"V{verificationNo}",
                Date = DateTime.Now.Subtract(TimeSpan.FromDays(19)),
                Description = "Du överför pengar från skattekonto till företagskonto",
                Attachment = String.Empty
            });

        context.Entries.AddRange(
            new Entry
            {
                AccountNo = 1630,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Credit = 4000m
            },
            new Entry
            {
                AccountNo = 1930,
                VerificationNo = $"V{verificationNo}",
                Description = string.Empty,
                Debit = 4000m
            });

        verificationNo++;
    }
}
