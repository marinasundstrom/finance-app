
using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions.Models;
using Transactions.Data;

namespace Transactions.Commands;

public record SetTransactionStatus(string TransactionId, TransactionStatus Status) : IRequest
{
    public class Handler : IRequestHandler<SetTransactionStatus>
    {
        private readonly ITransactionsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(ITransactionsContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(SetTransactionStatus request, CancellationToken cancellationToken)
        {

            var transaction = await _context.Transactions.FirstAsync(x => x.Id == request.TransactionId, cancellationToken);

            transaction.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}