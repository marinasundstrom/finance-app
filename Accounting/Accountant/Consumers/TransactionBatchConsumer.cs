using Accounting.Client;

using Invoices.Client;

using MassTransit;

using Transactions.Contracts;

namespace Transactions.Consumers;

public class TransactionBatchConsumer : IConsumer<TransactionBatch>
{
    private readonly IVerificationsClient _verificationsClient;
    private readonly IInvoicesClient _invoicesClient;

    public TransactionBatchConsumer(IVerificationsClient verificationsClient, IInvoicesClient invoicesClient)
    {
        _verificationsClient = verificationsClient;
        _invoicesClient = invoicesClient;
    }

    public async Task Consume(ConsumeContext<TransactionBatch> context)
    {
        var batch = context.Message;

        foreach (var transaction in batch.Transactions)
        {
            await HandleTransaction(transaction, context.CancellationToken);
        }
    }

    private async Task HandleTransaction(Transaction transaction, CancellationToken cancellationToken)
    {
        // Find invoice
        // Check invoice status
        // Is it underpaid?
        // Overpaid? Pay back

        // Create verification

        // Not found set transaction status unknown

        if (!int.TryParse(transaction.Reference, out var invoiceId))
        {
            // Mark transaction as unknown

            return;
        }

        var invoice = await _invoicesClient.GetInvoiceAsync(invoiceId, cancellationToken);

        if (invoice is null)
        {
            // Mark transaction as unknown
        }
        else
        {
            var receivedAmount = transaction.Amount;

            switch (invoice.Status)
            {
                case InvoiceStatus.Created:
                    // Do nothing
                    break;

                case InvoiceStatus.Sent:
                    if (receivedAmount < invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.PartiallyPaid, cancellationToken);
                    }
                    else if (receivedAmount == invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.Paid, cancellationToken);
                    }
                    else if (receivedAmount > invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.Overpaid, cancellationToken);
                    }
                    await _invoicesClient.SetPaidAmountAsync(invoice.Id, invoice.Paid.GetValueOrDefault() + receivedAmount);
                    break;

                case InvoiceStatus.Paid:
                case InvoiceStatus.PartiallyPaid:
                case InvoiceStatus.Overpaid:
                    var paidAmount = invoice.Paid.GetValueOrDefault() + receivedAmount;
                    if (paidAmount < invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.PartiallyPaid, cancellationToken);
                    }
                    else if (paidAmount == invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.Paid, cancellationToken);
                    }
                    else if (paidAmount > invoice.Total)
                    {
                        await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.Overpaid, cancellationToken);
                    }
                    await _invoicesClient.SetPaidAmountAsync(invoice.Id, paidAmount);
                    break;

                case InvoiceStatus.Cancelled:
                    // Mark transaktion for re-pay
                    break;
            }

            var verificationId = await _verificationsClient.CreateVerificationAsync(new CreateVerification
            {
                Description = $"Betalade faktura #{invoice.Id}",
                Entries = new[]
                {
                    new CreateEntry
                    {
                        AccountNo = 1920,
                        Description = string.Empty,
                        Debit = transaction.Amount
                    },
                    new CreateEntry
                    {
                        AccountNo = 1510,
                        Description = string.Empty,
                        Credit = transaction.Amount
                    }
                }
            }, cancellationToken);

            //await _verificationsClient.AddFileAttachmentToVerificationAsync(verificationId, new FileParameter());
        }
    }
}