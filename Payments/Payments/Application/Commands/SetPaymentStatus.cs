using MediatR;

using Microsoft.EntityFrameworkCore;

using Payments.Domain;
using Payments.Domain.Enums;

namespace Payments.Application.Commands;

public record SetPaymentStatus(string PaymentId, PaymentStatus Status) : IRequest
{
    public class Handler : IRequestHandler<SetPaymentStatus>
    {
        private readonly IPaymentsContext _context;
        
        public Handler(IPaymentsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetPaymentStatus request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FirstAsync(x => x.Id == request.PaymentId, cancellationToken);

            payment.SetStatus(request.Status);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
