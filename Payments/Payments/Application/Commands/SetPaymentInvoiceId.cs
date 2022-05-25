using MediatR;

using Microsoft.EntityFrameworkCore;

using Payments.Domain;

namespace Payments.Application.Commands;

public record SetPaymentInvoiceId(string PaymentId, int InvoiceId) : IRequest
{
    public class Handler : IRequestHandler<SetPaymentInvoiceId>
    {
        private readonly IPaymentsContext _context;
        
        public Handler(IPaymentsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetPaymentInvoiceId request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FirstAsync(x => x.Id == request.PaymentId, cancellationToken);

            payment.SetInvoiceId(request.InvoiceId);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}