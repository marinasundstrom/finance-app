using System;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions.Contracts;
using Transactions.Data;
using Transactions.Queries;

namespace Transactions.Queries;

public record GetTransactons(int Page, int PageSize) : IRequest<ItemsResult<TransactionDto>>
{
    public class Handler : IRequestHandler<GetTransactons, ItemsResult<TransactionDto>>
    {
        private readonly ITransactionsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(ITransactionsContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ItemsResult<TransactionDto>> Handle(GetTransactons request, CancellationToken cancellationToken)
        {
            var query = _context.Transactions
                .AsSplitQuery()
                .AsNoTracking()
                .OrderByDescending(x => x.Date)
                .AsQueryable();

            int totalItems = await query.CountAsync(cancellationToken);

            query = query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize);

            var items = await query.ToArrayAsync(cancellationToken);

            return new ItemsResult<TransactionDto>(
                items.Select(t => new TransactionDto(t.Id, t.Date, t.Status, t.From!, t.Reference!, t.Currency, t.Amount)), 
                totalItems);
        }
    }
}