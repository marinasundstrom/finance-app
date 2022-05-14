using System.ComponentModel.DataAnnotations;

namespace Accounting.Domain.Entities;

public class Verification
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public List<Entry> Entries { get; set; } = new List<Entry>();

    public List<Attachment> Attachments { get; set; } = new List<Attachment>();
}