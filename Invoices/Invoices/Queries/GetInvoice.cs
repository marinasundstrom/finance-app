using System;

using Invoices.Contracts;
using Invoices.Data;
using Invoices.Queries;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Queries;

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