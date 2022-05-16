using System;
namespace Invoices.Queries;

public record InvoiceDto(int Id, DateTime Date, Models.InvoiceStatus Status, decimal Total, decimal Vat, decimal VatRate, decimal? Paid);