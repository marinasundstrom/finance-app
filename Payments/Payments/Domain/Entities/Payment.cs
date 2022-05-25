using Payments.Domain.Common;
using Payments.Domain.Enums;
using Payments.Domain.Events;

namespace Payments.Domain.Entities;

public class Payment : IHasDomainEvents
{
    private Payment()
    {

    }

    public Payment(string? id, DateTime date, PaymentStatus status, string? from, string? reference, string currency, decimal amount)
    {
        if(amount <= 0) 
        {
            throw new ArgumentException("Amount must be greater than 0.");
        }

        Id = id ?? Guid.NewGuid().ToUrlFriendlyString();
        Date = date;
        Status = status;
        From = from;
        Reference = reference;
        Currency = currency;
        Amount = amount;

        DomainEvents.Add(new PaymentRegistered(Id));
    }

    public string Id { get; set; } = null!;

    public DateTime Date { get; set; }

    public PaymentStatus Status { get; private set; } = PaymentStatus.Unverified;

    public void UpdateReference(string reference)
    {
        if(Reference != reference) 
        {
            Reference = reference;
            DomainEvents.Add(new PaymentReferenceUpdated(Id, reference));
        }
    }

    public string? From { get; set; }

    public string? Reference { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public int? InvoiceId { get; private set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

    public void SetStatus(PaymentStatus status)
    {
        if(Status != status) 
        {
            Status = status;
            DomainEvents.Add(new PaymentStatusChanged(Id, status));
        }
    }

    public void SetInvoiceId(int invoiceId)
    {
        if(InvoiceId != invoiceId) 
        {
            InvoiceId = invoiceId;
            DomainEvents.Add(new PaymentInvoiceIdUpdated(Id, invoiceId));
        }
    }
}