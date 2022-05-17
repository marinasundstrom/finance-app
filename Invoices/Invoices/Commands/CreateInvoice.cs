using System;

using Invoices.Contracts;
using Invoices.Data;
using Invoices.Models;
using Invoices.Queries;

using MassTransit;

using MediatR;

namespace Invoices.Commands;

public record CreateInvoice(DateTime Date, InvoiceStatus Status, decimal SubTotal, decimal Vat, double VatRate, decimal Total) : IRequest<InvoiceDto>
{
    public class Handler : IRequestHandler<CreateInvoice, InvoiceDto>
    {
        private readonly IInvoicesContext _context;

        public Handler(IInvoicesContext context)
        {
            _context = context;
        }

        public async Task<InvoiceDto> Handle(CreateInvoice request, CancellationToken cancellationToken)
        {
            var invoice = new Invoices.Models.Invoice(request.Date, request.Status, request.SubTotal, request.Vat, request.VatRate, request.Total);

            _context.Invoices.Add(invoice);

            await _context.SaveChangesAsync(cancellationToken);

            return new InvoiceDto(invoice.Id, invoice.Date, invoice.Status, invoice.SubTotal, invoice.Vat, invoice.VatRate, invoice.Total, invoice.Paid);
        }
    }
}