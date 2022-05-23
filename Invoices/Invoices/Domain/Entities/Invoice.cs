using System;

using Invoices.Domain.Common;
using Invoices.Domain.Enums;
using Invoices.Domain.Events;

namespace Invoices.Domain.Entities;

public class Invoice : IHasDomainEvents
{
    readonly List<InvoiceItem> _items = new List<InvoiceItem>();

    private Invoice() { }

    public Invoice(DateTime? date, InvoiceStatus status = InvoiceStatus.Draft, string? note = null)
    {
        Date = date ?? DateTime.Now;
        Status = status;
        Note = note;

        DomainEvents.Add(new InvoiceCreated(Id));
    }

    public int Id { get; set; }

    public DateTime Date { get; set; }

    public InvoiceStatus Status { get; private set; }

    public DateTime? DueDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    public decimal? Paid { get; set; }

    public string? Note { get; set; }

    public IReadOnlyList<InvoiceItem> Items => _items;

    public InvoiceItem AddItem(ProductType productType, string description, decimal unitPrice, string unit, double vatRate, double quantity) 
    {
        var invoiceItem = new InvoiceItem(productType, description, unitPrice, unit, vatRate, quantity);
        _items.Add(invoiceItem);

        UpdateTotals();

        return invoiceItem;
    }

    internal void UpdateTotals()
    {
        Vat = Items.Sum(i => i.Vat());
        SubTotal = Items.Sum(i => i.SubTotal());
        Total = Items.Sum(i => i.LineTotal);
    }

    public void SetStatus(InvoiceStatus status)
    {
        if(Status != status) 
        {
            Status = status;
            DomainEvents.Add(new InvoiceStatusChanged(Id, Status));
        }
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
