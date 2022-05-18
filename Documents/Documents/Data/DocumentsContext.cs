using System;

using Documents.Models;

using Microsoft.EntityFrameworkCore;

namespace Documents.Data;

public class DocumentsContext : DbContext
{
    public DocumentsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = null!;
}

