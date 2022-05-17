using System;
namespace Invoices.Models;

public class Invoice
{
    private Invoice() { }

    public Invoice(DateTime date, InvoiceStatus status, decimal subTotal, decimal vat, double vatRate, decimal total)
    {
        Date = date;
        Status = status;
        SubTotal = subTotal;
        Vat = vat;
        VatRate = vatRate;
        Total = total;
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Vat { get; set; }
    public double VatRate { get; set; }
    public decimal Total { get; set; }
    public decimal? Paid { get; set; }
}