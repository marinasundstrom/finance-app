using System;
namespace Invoices.Queries;

public record InvoiceDto(int Id, DateTime Date, Models.InvoiceStatus Status, decimal SubTotal, decimal Vat, double VatRate, decimal Total, decimal? Paid);