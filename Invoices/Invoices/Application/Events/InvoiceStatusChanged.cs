using Invoices.Application.Common.Models;
using Invoices.Domain;
using Invoices.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Application.Events;

public class InvoiceStatusChangedHandler : INotificationHandler<DomainEventNotification<InvoiceStatusChanged>>
{
    private readonly IInvoicesContext _context;

    public InvoiceStatusChangedHandler(IInvoicesContext context)
    {
        _context = context;
    }

    public async Task Handle(DomainEventNotification<InvoiceStatusChanged> notification, CancellationToken cancellationToken)
    {
        var invoice = await _context
            .Invoices
            .FirstOrDefaultAsync(i => i.Id == notification.DomainEvent.InvoiceId);

        if(invoice is null) 
        {

        }
    }
}