using System;

using MassTransit;

using MediatR;

using Transactions.Contracts;
using Transactions.Data;
using Transactions.Queries;

namespace Transactions.Commands;

public record PostTransactions(IEnumerable<TransactionDto> Transactions) : IRequest
{
    public class Handler : IRequestHandler<PostTransactions>
    {
        private readonly ITransactionsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(ITransactionsContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(PostTransactions request, CancellationToken cancellationToken)
        {
            foreach (var transaction in request.Transactions)
            {
                _context.Transactions.Add(new Models.Transaction()
                {
                    Id = transaction.Id,
                    Date = transaction.Date ?? DateTime.Now,
                    Status = Models.TransactionStatus.Unverified,
                    From = transaction.From,
                    Reference = transaction.Reference,
                    Currency = transaction.Currency,
                    Amount = transaction.Amount
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(
                new TransactionBatch(request.Transactions.Select(t => new Contracts.Transaction(t.Id, t.Date.GetValueOrDefault(), (Contracts.TransactionStatus)t.Status, t.From, t.Reference, t.Currency, t.Amount))));

            return Unit.Value;
        }
    }
}