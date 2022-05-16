namespace Accountant.Services;

public class Invoice 
{
    public Invoice(int invoiceNo, DateTime date, InvoiceStatus status, decimal total, decimal vat, decimal vatRate)
    {
        InvoiceNo = invoiceNo;
        Date = date;
        Status = status;
        Total = total;
        Vat = vat;
        VatRate = vatRate;
    }

    public int InvoiceNo { get; set; }
    public DateTime Date { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal Total { get; set; }
    public decimal Vat { get; set; }
    public decimal VatRate { get; set; }

    public decimal? Paid { get; set; }
}
