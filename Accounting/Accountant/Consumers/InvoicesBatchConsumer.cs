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

        if(invoice.Status != InvoiceStatus.Sent) 
        {
            return;
        }

        var itemsGroupedByTypeAndVatRate = invoice.Items.GroupBy(item => new { item.ProductType, item.VatRate });

        List<CreateEntry> entries = new ();

        foreach(var group in itemsGroupedByTypeAndVatRate) 
        {
            var productType = group.Key.ProductType;
            var vatRate = group.Key.VatRate;

            var vat = group.Sum(i => i.LineTotal).Vat(vatRate);
            var subTotal = group.Sum(i => i.LineTotal).SubTotal(vatRate);

            entries.Add(new CreateEntry
            {
                AccountNo = 1510,
                Description = string.Empty,
                Debit = invoice.Total
            });

            if(productType == ProductType.Good) 
            {
                if(vatRate == 0.25) 
                {
                    entries.AddRange(new [] {
                        new CreateEntry
                        {
                            AccountNo = 2610,
                            Description = string.Empty,
                            Credit = vat
                        },
                        new CreateEntry
                        {
                            AccountNo = 3001,
                            Description = string.Empty,
                            Credit = subTotal
                        }
                    });
                }
                else if(vatRate == 0.12) 
                {
                    entries.AddRange(new [] {
                        new CreateEntry
                        {
                            AccountNo = 2610,
                            Description = string.Empty,
                            Credit = vat
                        },
                        new CreateEntry
                        {
                            AccountNo = 3002,
                            Description = string.Empty,
                            Credit = subTotal
                        }
                    });
                }
                else if(vatRate == 0.06) 
                {
                    entries.AddRange(new [] {
                        new CreateEntry
                        {
                            AccountNo = 2610,
                            Description = string.Empty,
                            Credit = vat
                        },
                        new CreateEntry
                        {
                            AccountNo = 3003,
                            Description = string.Empty,
                            Credit = subTotal
                        }
                    });
                }
            }
            else if(productType == ProductType.Service) 
            {
                if(vatRate == 0.25) 
                {
                    entries.AddRange(new [] {
                        new CreateEntry
                        {
                            AccountNo = 2610,
                            Description = string.Empty,
                            Credit = vat
                        },
                        new CreateEntry
                        {
                            AccountNo = 3041,
                            Description = string.Empty,
                            Credit = subTotal
                        }
                    });
                }
            }
        }

        await _verificationsClient.CreateVerificationAsync(new CreateVerification
        {
            Description = $"Skickade ut faktura #{i.Id}",
            Entries = entries
        }, cancellationToken);
    }
}