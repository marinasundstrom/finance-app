namespace Invoices.Contracts;

public record InvoicesBatch(IEnumerable<Invoice> Invoices);

public record Invoice(string Id, decimal Total, decimal Vat, decimal VatRate);