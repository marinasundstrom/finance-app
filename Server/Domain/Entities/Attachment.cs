using System.ComponentModel.DataAnnotations;

using Accounting.Domain.Enums;

namespace Accounting.Domain.Entities;

public class Attachment
{
    [Key]
    public string Id { get; set; } = null!;

    public AttachmentType Type { get; set; }

    public Verification Verification  { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public string? InvoiceNo { get; set; }

    public string? ReceiptNo { get; set; }
}