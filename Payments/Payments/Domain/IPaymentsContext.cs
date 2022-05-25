using Microsoft.EntityFrameworkCore;

using Payments.Domain.Entities;

namespace Payments.Domain;

public interface IPaymentsContext
{
    DbSet<Payment> Payments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}