using Documents.Domain.Common;

namespace Documents.Domain.Entities;

public class Document : AuditableEntity, IHasDomainEvents
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string BlobId { get; set; } = null!;

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}