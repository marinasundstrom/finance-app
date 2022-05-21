
using Invoices.Application;
using Invoices.Domain;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Queries;

public record GetInvoices(int Page, int PageSize) : IRequest<ItemsResult<InvoiceDto>>
{
    public class Handler : IRequestHandler<GetInvoices, ItemsResult<InvoiceDto>>
    {
        private readonly IInvoicesContext _context;

        public Handler(IInvoicesContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<InvoiceDto>> Handle(GetInvoices request, CancellationToken cancellationToken)
        {
            var query = _context.Invoices
                .AsSplitQuery()
                .AsNoTracking()
                .OrderByDescending(x => x.Date)
                .AsQueryable();

            int totalItems = await query.CountAsync(cancellationToken);

            query = query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize);

            var items = await query.ToArrayAsync(cancellationToken);

            return new ItemsResult<InvoiceDto>(
                items.Select(invoice => new InvoiceDto(invoice.Id, invoice.Date, invoice.Status, invoice.SubTotal, invoice.Vat, invoice.VatRate, invoice.Total, invoice.Paid)),
                totalItems);
        }
    }
}