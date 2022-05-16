using Accounting.Client;

using MassTransit;

using Transactions.Contracts;

namespace Transactions.Consumers;

public class TransactionBatchConsumer : IConsumer<TransactionBatch>
{
    private readonly IVerificationsClient _verificationsClient;

    public TransactionBatchConsumer(IVerificationsClient verificationsClient)
    {
        _verificationsClient = verificationsClient;
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

        var verificationId = await _verificationsClient.CreateVerificationAsync(new CreateVerification
        {
            Description = string.Empty,
            Entries = new[]
            {
                new CreateEntry
                {
                    AccountNo = 1920,
                    Description = string.Empty,
                    Debit = 10000m
                },
                new CreateEntry
                {
                    AccountNo = 1510,
                    Description = string.Empty,
                    Credit = 10000m
                }
            }
        });

        //await _verificationsClient.AddFileAttachmentToVerificationAsync(verificationId, new FileParameter());
    }
}