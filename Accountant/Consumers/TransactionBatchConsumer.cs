using Accountant.Services;

using Accounting.Client;

using MassTransit;

using Transactions.Contracts;

namespace Transactions.Consumers;

public class TransactionBatchConsumer : IConsumer<TransactionBatch>
{
    private readonly IVerificationsClient _verificationsClient;
    private readonly IInvoiceService _invoiceService;

    public TransactionBatchConsumer(IVerificationsClient verificationsClient, IInvoiceService invoiceService)
    {
        _verificationsClient = verificationsClient;
        _invoiceService = invoiceService;
    }

    public async Task Consume(ConsumeContext<TransactionBatch> context)
    {
        var batch = context.Message;

        foreach (var transaction in batch.Transactions)
        {
            await HandleTransaction(transaction);
        }
    }

    private async Task HandleTransaction(Transaction transaction)
    {
        // Find invoice
        // Check invoice status
        // Is it underpaid?
        // Overpaid? Pay back

        // Create verification

        // Not found set transaction status unknown

        if(!int.TryParse(transaction.Reference, out var invoiceId))
        {
            // Mark transaction as unknown

            return;
        }

        var invoice = await _invoiceService.GetInvoiceAsync(invoiceId);

        if(invoice is null)
        {
            // Mark transaction as unknown
        }
        else
        {
            var receivedAmount = transaction.Amount;

            switch(invoice.Status)
            {
                case InvoiceStatus.Created:
                    // Do nothing
                    break;

                case InvoiceStatus.Sent:
                    if (receivedAmount < invoice.Total)
                    {
                        await _invoiceService.SetInvoiceStatus(invoice.InvoiceNo, InvoiceStatus.PartiallyPaid);
                    }
                    else if (receivedAmount == invoice.Total)
                    {
                        await _invoiceService.SetInvoiceStatus(invoice.InvoiceNo, InvoiceStatus.Paid);
                    }
                    else if (receivedAmount > invoice.Total)
                    {
                        await _invoiceService.SetInvoiceStatus(invoice.InvoiceNo, InvoiceStatus.Overpaid);
                    }
                    invoice.Paid = receivedAmount;

                    break;

                case InvoiceStatus.Paid:
                    await _invoiceService.SetInvoiceStatus(invoice.InvoiceNo, InvoiceStatus.Overpaid);
                    invoice.Paid += receivedAmount;
                    break;

                case InvoiceStatus.Cancelled:
                    // Mark transaktion for re-pay
                    break;
            }

            var verificationId = await _verificationsClient.CreateVerificationAsync(new CreateVerification
            {
                Description = $"Betalar faktura #{invoice.InvoiceNo}",
                Entries = new[]
                {
                    new CreateEntry
                    {
                        AccountNo = 1920,
                        Description = string.Empty,
                        Debit = invoice.Total
                    },
                    new CreateEntry
                    {
                        AccountNo = 1510,
                        Description = string.Empty,
                        Credit = invoice.Total
                    }
                }
            });

            //await _verificationsClient.AddFileAttachmentToVerificationAsync(verificationId, new FileParameter());
        }
    }
}