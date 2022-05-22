using System;

using Invoices.Domain.Common;
using Invoices.Domain.Enums;
using Invoices.Domain.Events;

namespace Invoices.Domain.Entities;

public class Invoice : IHasDomainEvent
{
    private Invoice() { }

    public Invoice(DateTime date, InvoiceStatus status, decimal subTotal, decimal vat, double vatRate, decimal total)
    {
        if(total != subTotal + vat) 
        {
            throw new Exception("Total is not equal to Sub Total + VAT.");
        }

        Date = date;
        Status = status;
        SubTotal = subTotal;
        Vat = vat;
        VatRate = vatRate;
        Total = total;

        DomainEvents.Add(new InvoiceCreated(Id));
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public InvoiceStatus Status { get; private set; }
    public decimal SubTotal { get; set; }
    public decimal Vat { get; set; }
    public double VatRate { get; set; }
    public decimal Total { get; set; }
    public decimal? Paid { get; set; }

    public void SetStatus(InvoiceStatus status)
    {
        Status = status;
        DomainEvents.Add(new InvoiceStatusChanged(Id, Status));
    }
 
    /*
    public void SetPaidAmount(decimal amount)
    {
        Paid = amount;

        if(Paid == Total) 
        {
            SetStatus(InvoiceStatus.Paid);
        }
        else if(Paid < Total) 
        {
            SetStatus(InvoiceStatus.PartiallyPaid);
        }
        else if(Paid > Total) 
        {
            SetStatus(InvoiceStatus.Overpaid);
        }
    }
    */

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}