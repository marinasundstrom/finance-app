using Accounting.Client;

using Invoices.Client;

namespace Accountant.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IInvoicesClient _invoicesClient;
        private readonly IVerificationsClient _verificationsClient;
        private readonly ILogger<RefundService> _logger;

        public ReminderService(IInvoicesClient invoicesClient, IVerificationsClient verificationsClient, ILogger<RefundService> logger)
        {
            _invoicesClient = invoicesClient;
            _verificationsClient = verificationsClient;
            _logger = logger;
        }

        public async Task IssueReminders()
        {
            _logger.LogInformation("Querying for invoices");

            var results = await _invoicesClient.GetInvoicesAsync(0, 100, new [] { InvoiceStatus.PartiallyPaid, InvoiceStatus.Sent });

            foreach(var invoice in results.Items)
            {
                if (invoice.Status == InvoiceStatus.PartiallyPaid) 
                {
                    // TODO: Move to its own scheduled task

                    _logger.LogDebug($"Notify customer about partially paid invoice {invoice.Id}");

                    // Send email
                }
                else if (invoice.Status == InvoiceStatus.Sent)
                {
                    // TODO: Move to its own scheduled task

                    var daysSince = (DateTime.Now.Date - invoice.Date.Date).TotalDays;
                    if(daysSince > 30) 
                    {
                        _logger.LogDebug($"Notify customer about forgotten invoice {invoice.Id}");

                        // Send email
                    }
                }
            }
        }
    }
}

