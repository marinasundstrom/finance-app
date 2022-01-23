using System.ComponentModel.DataAnnotations;

namespace Accounting.Application.Verifications;

public class CreateVerification
{
    public string Description { get; set; } = null!;

    [Required]
    public List<CreateEntry> Entries { get; set; } = new List<CreateEntry>()!;
}
