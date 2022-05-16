using Accounting.Client;

using Invoices.Contracts;

using MassTransit;

namespace Transactions.Consumers;

public class InvoicesBatchConsumer : IConsumer<InvoicesBatch>
{
    private readonly IVerificationsClient _verificationsClient;

    public InvoicesBatchConsumer(IVerificationsClient verificationsClient)
    {
        _verificationsClient = verificationsClient;
    }

    public async Task Consume(ConsumeContext<InvoicesBatch> context)
    {
        var batch = context.Message;

        foreach (var invoice in batch.Invoices)
        {
            await HandleInvoice(invoice);
        }
    }

    private async Task HandleInvoice(Invoice invoice)
    {
        // Get invoice
        // Register entries

        await _verificationsClient.CreateVerificationAsync(new CreateVerification
        {
            Description = string.Empty,
            Entries = new[]
            {
                new CreateEntry
                {
                    AccountNo = 1510,
                    Description = string.Empty,
                    Debit = 10000m
                },
                new CreateEntry
                {
                    AccountNo = 2610,
                    Description = string.Empty,
                    Credit = 2000m
                },
                new CreateEntry
                {
                    AccountNo = 3001,
                    Description = string.Empty,
                    Credit = 8000m
                }
            }
        });
    }
}