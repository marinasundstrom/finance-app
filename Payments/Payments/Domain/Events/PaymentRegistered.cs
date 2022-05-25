using Payments.Domain.Common;

namespace Payments.Domain.Events;

public class PaymentRegistered : DomainEvent
{
    public PaymentRegistered(string paymentId)
    {
        PaymentId = paymentId;
    }

    public string PaymentId { get; }
}