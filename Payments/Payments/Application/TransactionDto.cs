using Payments.Domain.Enums;

namespace Payments.Application;

public record PaymentDto(string Id, DateTime? Date, PaymentStatus Status, string From, string Reference, string Currency, decimal Amount, int? InvoiceId);