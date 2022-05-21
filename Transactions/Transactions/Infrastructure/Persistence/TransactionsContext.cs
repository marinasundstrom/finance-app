using Microsoft.EntityFrameworkCore;

using Transactions.Domain;
using Transactions.Domain.Entities;

namespace Transactions.Infrastructure.Persistence;

public class TransactionsContext : DbContext, ITransactionsContext
{
    public TransactionsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;
}