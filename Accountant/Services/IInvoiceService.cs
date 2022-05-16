namespace Accountant.Services;

public interface IInvoiceService
{
    Task<IEnumerable<Invoice>> GetInvoicesAsync();

    Task<Invoice?> GetInvoiceAsync(int invoiceNo);

    Task SetInvoiceStatus(int invoiceNo, InvoiceStatus status);
}
