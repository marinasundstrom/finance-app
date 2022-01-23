using System;
using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Entries.Queries
{
	public class GetEntriesQuery : IRequest<EntriesResult>
	{
		public GetEntriesQuery(int? accountNo = null, string? verificationNo = null, int page = 0, int pageSize = 10, ResultDirection direction = ResultDirection.Asc)
		{
            AccountNo = accountNo;
            VerificationNo = verificationNo;
            Page = page;
            PageSize = pageSize;
            Direction = direction;
        }

        public int? AccountNo { get; }

        public string? VerificationNo { get; }

        public int Page { get; }

        public int PageSize { get; }

        public ResultDirection Direction { get; }

        public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, EntriesResult>
        {
            private readonly IAccountingContext context;

            public GetEntriesQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<EntriesResult> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
            {
                var query = context.Entries
                       .Include(e => e.Verification)
                       .Include(e => e.Account)
                       .AsNoTracking()
                       .AsQueryable();

                if (request.Direction == ResultDirection.Asc)
                {
                    query = query.OrderBy(e => e.Date);
                }
                else
                {
                    query = query.OrderByDescending(e => e.Date);
                }

                if (request.AccountNo is not null)
                {
                    query = query.Where(e => e.AccountNo == request.AccountNo);
                }

                if (request.VerificationNo is not null)
                {
                    query = query.Where(e => e.VerificationNo == request.VerificationNo);
                }

                var totalItems = await query.CountAsync(cancellationToken);

                var entries = await query
                    .Skip(request.PageSize * request.Page)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var entries2 = entries.Select(e => new EntryDto(
                    e.Id,
                    e.Date,
                    new VerificationShort
                    {
                        VerificationNo = e.VerificationNo,
                        Date = e.Verification.Date,
                        Description = e.Verification.Description,
                    },
                    new AccountShortDto
                    {
                        AccountNo = e.AccountNo,
                        Name = e.Account.Name
                    },
                    e.Description,
                    e.Debit,
                    e.Credit
                ));

                return new EntriesResult(entries2, totalItems);
            }
        }
    }
}

