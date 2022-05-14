using Accounting.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Verifications.Queries
{
    public class GetVerificationsQuery : IRequest<VerificationsResult>
    {
        public GetVerificationsQuery(int page = 0, int pageSize = 10)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }

        public int PageSize { get; }

        public class GetVerificationsQueryHandler : IRequestHandler<GetVerificationsQuery, VerificationsResult>
        {
            private readonly IAccountingContext context;

            public GetVerificationsQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<VerificationsResult> Handle(GetVerificationsQuery request, CancellationToken cancellationToken)
            {
                var query = context.Verifications
                    .Include(x => x.Entries)
                    .Include(x => x.Attachments)
                    .OrderBy(x => x.Date)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable();

                var totalItems = await query.CountAsync();

                var r = await query
                   .Skip(request.PageSize * request.Page)
                   .Take(request.PageSize)
                   .ToListAsync(cancellationToken);

                var vms = new List<VerificationDto>();

                vms.AddRange(r.Select(v => v.ToDto()));

                return new VerificationsResult(vms, totalItems);
            }
        }
    }
}