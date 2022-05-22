using Microsoft.AspNetCore.SignalR;

using Transactions.Domain.Enums;
using Transactions.Hubs;

namespace Transactions.Application.Services;

public class TransactionsHubClient : ITransactionsHubClient
{
    private readonly IHubContext<TransactionsHub, ITransactionsHubClient> _hubContext;

    public TransactionsHubClient(IHubContext<TransactionsHub, ITransactionsHubClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task TransactionUpdated(TransactionUpdatedDto dto)
    {
        await _hubContext.Clients.All.TransactionUpdated(dto);
    }
}