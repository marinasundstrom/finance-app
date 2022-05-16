using System;

using Transactions.Models;

using Microsoft.EntityFrameworkCore;

namespace Transactions.Data;

public class TransactionsContext : DbContext, ITransactionsContext
{
    public TransactionsContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;
}

