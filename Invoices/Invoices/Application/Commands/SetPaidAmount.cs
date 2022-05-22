using Invoices.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Invoices.Application.Commands;

public record SetPaidAmount(int InvoiceId, decimal Amount) : IRequest
{
    public class Handler : IRequestHandler<SetPaidAmount>
    {
        private readonly IInvoicesContext _context;

        public Handler(IInvoicesContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetPaidAmount request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.FirstAsync(x => x.Id == request.InvoiceId, cancellationToken);

            invoice.Paid = request.Amount;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}