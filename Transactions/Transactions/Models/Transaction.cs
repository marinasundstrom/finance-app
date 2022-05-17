using System;
namespace Transactions.Models;

public class Transaction
{
    public string Id { get; set; } = null!;

    public TransactionStatus Status { get; set; }

    public string? From { get; set; }

    public string? Reference { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }
}