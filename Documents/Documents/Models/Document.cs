namespace Documents.Models;

public class Document 
{
     public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string BlobId { get; set; } = null!;

    public DateTime Uploaded { get; set; }
}