using System;
namespace Invoices.Models;

public class Invoice
{
    private Invoice() { }

    public Invoice(DateTime date, InvoiceStatus status, decimal total, decimal vat, decimal vatRate)
    {
        Date = date;
        Status = status;
        Total = total;
        Vat = vat;
        VatRate = vatRate;
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal Total { get; set; }
    public decimal Vat { get; set; }
    public decimal VatRate { get; set; }

    public decimal? Paid { get; set; }
}