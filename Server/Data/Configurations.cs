using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Server.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", t => t.IsTemporal());
        //builder.HasQueryFilter(i => i.Deleted == null);

        builder.HasKey(x => x.AccountNo);

        builder
            .Property(x => x.AccountNo)
            .ValueGeneratedNever();
    }
}

public class Verificationfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("Verifications", t => t.IsTemporal());
    }
}

public class Entryfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder.ToTable("Entries", t => t.IsTemporal());
    }
}