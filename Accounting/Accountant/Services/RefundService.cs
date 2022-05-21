using System;

using Accounting.Client;

using Invoices.Client;

namespace Accountant.Services
{
    public class RefundService : IRefundService
    {
        private readonly IInvoicesClient _invoicesClient;
        private readonly IVerificationsClient _verificationsClient;
        private readonly ILogger<RefundService> _logger;

        public RefundService(IInvoicesClient invoicesClient, IVerificationsClient verificationsClient, ILogger<RefundService> logger)
        {
            _invoicesClient = invoicesClient;
            _verificationsClient = verificationsClient;
            _logger = logger;
        }

        public async Task CheckForRefund()
        {
            _logger.LogInformation("Querying for invoices");

            var results = await _invoicesClient.GetInvoicesAsync(0, 100);

            foreach(var invoice in results.Items)
            {
                if(invoice.Status == InvoiceStatus.Overpaid)
                {
                    _logger.LogDebug($"Handling overpaid invoice {invoice.Id}");

                    var amountToRefund = invoice.Paid - invoice.Total;
                    var vat = amountToRefund / (1m + (25m / 100m));

                    var verificationId = await _verificationsClient.CreateVerificationAsync(new CreateVerification
                    {
                        Description = $"Betalade tillbaka för #{invoice.Id}",
                        Entries = new[]
                        {
                            new CreateEntry
                            {
                                AccountNo = 1510,
                                Description = string.Empty,
                                Credit = amountToRefund
                            },
                            new CreateEntry
                            {
                                AccountNo = 2610,
                                Description = string.Empty,
                                Debit = vat
                            },
                            new CreateEntry
                            {
                                AccountNo = 3001,
                                Description = string.Empty,
                                Debit = amountToRefund - vat
                            }
                        }
                    });

                    await _invoicesClient.SetInvoiceStatusAsync(invoice.Id, InvoiceStatus.PartiallyRepaid);
                }
                else if (invoice.Status == InvoiceStatus.PartiallyPaid) 
                {
                    _logger.LogDebug($"Notify customer about partially paid invoice {invoice.Id}");
                }
                else if (invoice.Status == InvoiceStatus.Sent)
                {
                    var daysSince = (DateTime.Now.Date - invoice.Date.Date).TotalDays;
                    if(daysSince > 30) 
                    {
                        _logger.LogDebug($"Notify customer about forgotten invoice {invoice.Id}");
                    }
                }
            }
        }
    }
}

