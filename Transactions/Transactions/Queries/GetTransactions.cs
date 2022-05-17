using System;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions.Contracts;
using Transactions.Data;
using Transactions.Queries;

namespace Transactions.Queries;

public record GetTransactons() : IRequest<IEnumerable<TransactionDto>>
{
    public class Handler : IRequestHandler<GetTransactons, IEnumerable<TransactionDto>>
    {
        private readonly ITransactionsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(ITransactionsContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(GetTransactons request, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions.ToArrayAsync();
            return transactions.Select(t => new TransactionDto(t.Id, t.Date, t.Status, t.From!, t.Reference!, t.Currency, t.Amount));
        }
    }
}