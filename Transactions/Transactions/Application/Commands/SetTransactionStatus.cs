
using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions.Domain;
using Transactions.Domain.Enums;
using Transactions.Hubs;

namespace Transactions.Application.Commands;

public record SetTransactionStatus(string TransactionId, TransactionStatus Status) : IRequest
{
    public class Handler : IRequestHandler<SetTransactionStatus>
    {
        private readonly ITransactionsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITransactionsHubClient _transactionsHubClient;

        public Handler(ITransactionsContext context, IPublishEndpoint publishEndpoint, ITransactionsHubClient transactionsHubClient)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _transactionsHubClient = transactionsHubClient;
        }

        public async Task<Unit> Handle(SetTransactionStatus request, CancellationToken cancellationToken)
        {

            var transaction = await _context.Transactions.FirstAsync(x => x.Id == request.TransactionId, cancellationToken);

            transaction.SetStatus(request.Status);

            await _context.SaveChangesAsync(cancellationToken);

            await _transactionsHubClient.TransactionUpdated(new TransactionUpdatedDto(request.TransactionId, request.Status));

            return Unit.Value;
        }
    }
}