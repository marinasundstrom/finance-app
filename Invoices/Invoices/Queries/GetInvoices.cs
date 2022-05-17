
using Invoices.Data;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Queries;

public record GetInvoices() : IRequest<IEnumerable<InvoiceDto>>
{
    public class Handler : IRequestHandler<GetInvoices, IEnumerable<InvoiceDto>>
    {
        private readonly IInvoicesContext _context;

        public Handler(IInvoicesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoices request, CancellationToken cancellationToken)
        {
            var invoices = await _context.Invoices.ToArrayAsync(cancellationToken);

            return invoices.Select(invoice => new InvoiceDto(invoice.Id, invoice.Date, invoice.Status, invoice.Total, invoice.Vat, invoice.VatRate, invoice.Paid));
        }
    }
}