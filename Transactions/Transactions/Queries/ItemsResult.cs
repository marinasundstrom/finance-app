namespace Transactions.Queries;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);