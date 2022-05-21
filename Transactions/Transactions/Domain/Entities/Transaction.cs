using Transactions.Domain.Enums;

namespace Transactions.Domain.Entities;

public class Transaction
{
    private Transaction()
    {

    }

    public Transaction(string? id, DateTime date, TransactionStatus status, string? from, string? reference, string currency, decimal amount)
    {
        Id = id ?? Guid.NewGuid().ToUrlFriendlyString();
        Date = date;
        Status = status;
        From = from;
        Reference = reference;
        Currency = currency;
        Amount = amount;
    }

    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public TransactionStatus Status { get; private set; } = TransactionStatus.Unverified;

    public string? From { get; set; }

    public string? Reference { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public void SetStatus(TransactionStatus status)
    {
        Status = status;
    }
}