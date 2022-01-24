using System.ComponentModel.DataAnnotations;

namespace Accounting.Domain.Entities;

public class Verification
{
    [Key]
    public string VerificationNo { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public string Attachment { get; set; } = null!;

    public List<Entry> Entries { get; set; } = new List<Entry>();
}