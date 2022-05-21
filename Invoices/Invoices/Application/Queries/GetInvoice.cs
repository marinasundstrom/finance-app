using Invoices.Application.Queries;
using Invoices.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Application.Queries;

public record GetInvoice(int InvoiceId) : IRequest<InvoiceDto?>
{
    public class Handler : IRequestHandler<GetInvoice, InvoiceDto?>
    {
        private readonly IInvoicesContext _context;

        public Handler(IInvoicesContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto?> Handle(GetInvoice request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == request.InvoiceId, cancellationToken);

            return invoice is null
                ? null
                : new InvoiceDto(invoice.Id, invoice.Date, invoice.Status, invoice.SubTotal, invoice.Vat, invoice.VatRate, invoice.Total, invoice.Paid);
        }
    }
}