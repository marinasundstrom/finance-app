using Documents.Domain.Common;
using Documents.Domain.Events;

namespace Documents.Domain.Entities;

public class Document : AuditableEntity, IHasDomainEvents
{
    private Document() 
    {
    }

    public Document(string title) 
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        
        DomainEvents.Add(new DocumentCreated(Id));
    }

    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string BlobId { get; set; } = null!;

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}