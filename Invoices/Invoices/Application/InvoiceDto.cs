using System;
namespace Invoices.Application;

public record InvoiceDto(int Id, DateTime Date, Domain.Enums.InvoiceStatus Status, IEnumerable<InvoiceItemDto> Items, decimal SubTotal, decimal Vat, double? VatRate, decimal Total, decimal? Paid);

public record InvoiceItemDto(int Id, string Description, decimal UnitPrice, double VatRate, double Quantity, decimal LineTotal);