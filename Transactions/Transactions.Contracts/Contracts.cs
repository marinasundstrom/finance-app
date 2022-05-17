namespace Transactions.Contracts;

public record TransactionBatch(IEnumerable<Transaction> Transactions);

public record Transaction(string Id, string From, string Reference, string Currency, decimal Amount);