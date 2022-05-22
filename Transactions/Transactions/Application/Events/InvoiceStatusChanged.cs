using Transactions.Application.Common.Models;
using Transactions.Domain;
using Transactions.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Transactions.Hubs;

namespace Transactions.Application.Events;

public class TransactionStatusChangedHandler : INotificationHandler<DomainEventNotification<TransactionStatusChanged>>
{
    private readonly ITransactionsContext _context;
    private readonly ITransactionsHubClient _transactionsHubClient;

    public TransactionStatusChangedHandler(ITransactionsContext context, ITransactionsHubClient transactionsHubClient)
    {
        _context = context;
        _transactionsHubClient = transactionsHubClient;
    }

    public async Task Handle(DomainEventNotification<TransactionStatusChanged> notification, CancellationToken cancellationToken)
    {
        var transaction = await _context
            .Transactions
            .FirstOrDefaultAsync(i => i.Id == notification.DomainEvent.TransactionId);

        if(transaction is not null) 
        {
            await _transactionsHubClient.TransactionUpdated(new TransactionUpdatedDto(transaction.Id, transaction.Status));
        }
    }
}