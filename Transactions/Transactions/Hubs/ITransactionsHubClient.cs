using Transactions.Domain.Enums;

namespace Transactions.Hubs;

public interface ITransactionsHubClient
{
    Task TransactionUpdated(TransactionUpdatedDto dto);
}

public record TransactionUpdatedDto (string Id, TransactionStatus Status);