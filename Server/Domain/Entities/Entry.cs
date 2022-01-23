using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Domain.Entities;

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