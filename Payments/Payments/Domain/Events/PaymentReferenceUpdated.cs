using Payments.Domain.Common;

namespace Payments.Domain.Events;

public class PaymentReferenceUpdated : DomainEvent
{
    public PaymentReferenceUpdated(string paymentId, string reference)
    {
        PaymentId = paymentId;
        Reference = reference;
    }

    public string PaymentId { get; }

    public string Reference { get; }
}