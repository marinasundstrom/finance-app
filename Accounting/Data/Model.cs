using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Data;

public class Account
{
    [Key]
    public int AccountNo { get; set; }

    public AccountClass Class { get; set; }

    public AccountGroup Group { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<Entry> Entries { get; set; } = new List<Entry>();
}

public enum AccountClass
{
    Assets = 1,
    EquityAndLiabilites = 2,
    OperatingIncomeRevenue = 3,
    Costs = 4,
    OtherOperatingExpenses1 = 5,
    OtherOperatingExpenses2 = 6,
    PersonnelCosts = 7,
    FinancialAndOtherIcomeAndExpenses = 8
}

public enum AccountGroup
{
    Unspecified = 0
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