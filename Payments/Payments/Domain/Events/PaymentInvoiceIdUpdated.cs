using Payments.Domain.Common;

namespace Payments.Domain.Events;

public class PaymentInvoiceIdUpdated : DomainEvent
{
    public PaymentInvoiceIdUpdated(string paymentId, int invoiceId)
    {
        PaymentId = paymentId;
        InvoiceId = invoiceId;
    }

    public string PaymentId { get; }

    public int InvoiceId { get; }
}
