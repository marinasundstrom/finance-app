using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Common.Interfaces;

public interface IAccountingContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<Verification> Verifications { get; set; }
    DbSet<Entry> Entries { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
