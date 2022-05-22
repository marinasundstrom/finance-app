using Transactions.Domain.Enums;

namespace Transactions.Hubs;

public interface ITransactionsHubClient
{
    Task TransactionStatusUpdated(string Id, TransactionStatus Status);
}