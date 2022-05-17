using System;
namespace Transactions.Queries;

public record TransactionDto(string Id, string From, string Reference, string Currency, decimal Amount);