using System;

using Documents.Domain;
using Documents.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Documents.Infrastructure.Persistence;

public class DocumentsContext : DbContext, IDocumentsContext
{
    public DocumentsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Document> Documents { get; set; } = null!;

    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = null!;
}

