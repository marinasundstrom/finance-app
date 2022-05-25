using Payments.Domain.Enums;

namespace Payments.Hubs;

public interface IPaymentsHubClient
{
    Task PaymentStatusUpdated(string id, PaymentStatus Status);

    Task PaymentInvoiceIdUpdated(string id, int? invoiceId);
}