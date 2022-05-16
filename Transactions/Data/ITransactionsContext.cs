using Microsoft.EntityFrameworkCore;

using Transactions.Models;

namespace Transactions.Data;

public interface ITransactionsContext
{
    DbSet<Transaction> Transactions { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
