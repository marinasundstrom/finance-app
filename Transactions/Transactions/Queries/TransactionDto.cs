using System;

using Transactions.Models;

namespace Transactions.Queries;

public record TransactionDto(string Id, DateTime? Date, TransactionStatus Status, string From, string Reference, string Currency, decimal Amount);