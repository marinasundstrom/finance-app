using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions.Domain;
using Transactions.Domain.Enums;

namespace Transactions.Application.Queries;

public record GetTransactons(int Page, int PageSize, TransactionStatus[]? Status = null) : IRequest<ItemsResult<TransactionDto>>
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

            if (request.Status?.Any() ?? false)
            {
                var statuses = request.Status.Select(x => (int)x);
                query = query.Where(i => statuses.Any(s => s == (int)i.Status));
            }

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