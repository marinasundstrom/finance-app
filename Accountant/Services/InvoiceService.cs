namespace Accountant.Services;

public class InvoiceService : IInvoiceService
{
    readonly IEnumerable<Invoice> invoices = new [] {
        new Invoice(1001, DateTime.Now, InvoiceStatus.Sent, 100, 25, 25),
        new Invoice(1002, DateTime.Now, InvoiceStatus.Sent, 100, 25, 25)
    };

    public Task<IEnumerable<Invoice>> GetInvoicesAsync()
    {
        return Task.FromResult(invoices);
    }

    public Task<Invoice?> GetInvoiceAsync(int invoiceNo)
    {
        return Task.FromResult(invoices.FirstOrDefault(i => i.InvoiceNo == invoiceNo));
    }

    public Task SetInvoiceStatus(int invoiceNo, InvoiceStatus status) 
    {
        var invoice = invoices.First(i => i.InvoiceNo == invoiceNo);
        invoice.Status = status;
        return Task.CompletedTask;
    }
}
