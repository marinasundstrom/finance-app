using Accounting.Client;

using Invoices.Client;
using Invoices.Contracts;

using MassTransit;

namespace Accountant.Consumers;

public class InvoicesBatchConsumer : IConsumer<InvoicesBatch>
{
    private readonly IVerificationsClient _verificationsClient;
    private readonly IInvoicesClient _invoicesClient;

    public InvoicesBatchConsumer(IVerificationsClient verificationsClient, IInvoicesClient invoicesClient)
    {
        _verificationsClient = verificationsClient;
        _invoicesClient = invoicesClient;
    }

    public async Task Consume(ConsumeContext<InvoicesBatch> context)
    {
        var batch = context.Message;

        foreach (var invoice in batch.Invoices)
        {
            await HandleInvoice(invoice, context.CancellationToken);
        }
    }

    private async Task HandleInvoice(Invoice i, CancellationToken cancellationToken)
    {
        // Get invoice
        // Register entries

        var invoice = await _invoicesClient.GetInvoiceAsync(i.Id, cancellationToken);

        await _verificationsClient.CreateVerificationAsync(new CreateVerification
        {
            Description = $"Skickade ut faktura #{i.Id}",
            Entries = new[]
            {
                new CreateEntry
                {
                    AccountNo = 1510,
                    Description = string.Empty,
                    Debit = invoice.Total
                },
                new CreateEntry
                {
                    AccountNo = 2610,
                    Description = string.Empty,
                    Credit = invoice.Vat
                },
                new CreateEntry
                {
                    AccountNo = 3001,
                    Description = string.Empty,
                    Credit = invoice.Total - invoice.Vat
                }
            }
        }, cancellationToken);
    }
}