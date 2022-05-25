
using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Payments.Domain;

namespace Payments.Application.Commands;

public record UpdatePaymentReference(string PaymentId, string Reference) : IRequest
{
    public class Handler : IRequestHandler<UpdatePaymentReference>
    {
        private readonly IPaymentsContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        
        public Handler(IPaymentsContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Unit> Handle(UpdatePaymentReference request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FirstAsync(x => x.Id == request.PaymentId, cancellationToken);

            if(payment.Status != Domain.Enums.PaymentStatus.Unknown
                && payment.Status != Domain.Enums.PaymentStatus.Unverified) 
            {
                throw new Exception("Cannot change reference.");
            }

            payment.UpdateReference(request.Reference);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}