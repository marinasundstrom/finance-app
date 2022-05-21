using System;
namespace Invoices.Application;

public record InvoiceDto(int Id, DateTime Date, Domain.Enums.InvoiceStatus Status, decimal SubTotal, decimal Vat, double VatRate, decimal Total, decimal? Paid);