using System;

using Microsoft.EntityFrameworkCore;

using Transactions.Models;

namespace Transactions.Data;

public class TransactionsContext : DbContext, ITransactionsContext
{
    public TransactionsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;
}