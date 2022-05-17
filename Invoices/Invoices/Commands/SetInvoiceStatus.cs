
using MediatR;
using Invoices.Data;
using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using Invoices.Contracts;
using MassTransit;

namespace Invoices.Commands;

public record SetInvoiceStatus(int InvoiceId, InvoiceStatus Status) : IRequest
{
    public class Handler : IRequestHandler<SetInvoiceStatus>
    {
        private readonly IInvoicesContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public Handler(IInvoicesContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(SetInvoiceStatus request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.FirstAsync(x => x.Id == request.InvoiceId, cancellationToken);

            invoice.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            if(invoice.Status == InvoiceStatus.Sent) 
            {
                await _publishEndpoint.Publish(new InvoicesBatch(new[]
                {
                    new Contracts.Invoice(invoice.Id)
                }));
            }

            return Unit.Value;
        }
    }
}