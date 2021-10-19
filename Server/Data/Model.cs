using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Server.Data;

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
    [Display(Name = "Assets")]
    Assets = 1,

    [Display(Name = "Equity and liabilities")]
    EquityAndLiabilites = 2,

    [Display(Name = "Operating income/revenue")]
    OperatingIncomeRevenue = 3,

    [Display(Name = "Cost of goods, materials and certain sub-contract work")]
    Costs = 4,

    [Display(Name = "Other external operating expenses/costs (5)")]
    OtherOperatingExpenses1 = 5,

    [Display(Name = "Other external operating expenses/costs (6)")]
    OtherOperatingExpenses2 = 6,

    [Display(Name = "Personnel costs, depreciation etc.")]
    PersonnelCosts = 7,

    [Display(Name = "Financial and other income and expenses")]
    FinancialAndOtherIncomeAndExpenses = 8
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

    public DateTime Date { get; set; } = DateTime.Now;

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