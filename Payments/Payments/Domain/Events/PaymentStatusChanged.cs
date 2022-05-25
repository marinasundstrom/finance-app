using Payments.Domain.Common;
using Payments.Domain.Enums;

namespace Payments.Domain.Events;

public class PaymentStatusChanged : DomainEvent
{
    public PaymentStatusChanged(string paymentId, PaymentStatus status)
    {
        PaymentId = paymentId;
        Status = status;
    }

    public string PaymentId { get; }

    public PaymentStatus Status { get; }
}