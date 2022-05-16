namespace Accountant.Services;

public enum InvoiceStatus 
{
    Created,
    Sent,
    Paid,
    PartiallyPaid,
    Overpaid,
    Cancelled
}