using Payments.Application.Common.Models;
using Payments.Domain;
using Payments.Domain.Events;

using MediatR;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Payments.Application.Events;

public class PaymentReferenceUpdatedHandler : INotificationHandler<DomainEventNotification<PaymentReferenceUpdated>>
{
    private readonly IPaymentsContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public PaymentReferenceUpdatedHandler(IPaymentsContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DomainEventNotification<PaymentReferenceUpdated> notification, CancellationToken cancellationToken)
    {
        var t = await _context
            .Payments
            .FirstOrDefaultAsync(i => i.Id == notification.DomainEvent.PaymentId);

        if(t is not null) 
        {
            await _publishEndpoint.Publish(
                new Contracts.PaymentBatch(new [] { new Contracts.Payment(t.Id, t.Date, (Contracts.PaymentStatus)t.Status, t.From!, t.Reference!, t.Currency, t.Amount) }));
        }
    }
}