using System;
using Accounting.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Accounts.Mappings;

namespace Accounting.Application.Accounts.Queries
{
	public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>>
	{
		public GetAccountsQuery(int? accountClass = null, bool? showUnusedAccounts = false)
		{
            AccountClass = accountClass;
            ShowUnusedAccounts = showUnusedAccounts;
        }

        public int? AccountClass { get; }

        public bool? ShowUnusedAccounts { get; }

        public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
        {
            private readonly IAccountingContext context;

            public GetAccountsQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
            {
                var query = context.Accounts
                                .Include(a => a.Entries)
                                .AsNoTracking()
                                .AsQueryable();

                if (!request.ShowUnusedAccounts.GetValueOrDefault())
                {
                    query = query.Where(a => a.Entries.Any());
                }

                if (request.AccountClass is not null)
                {
                    query = query.Where(a => a.Class == (Domain.Enums.AccountClass)request.AccountClass);
                }

                var r = await query.ToListAsync(cancellationToken);

                var vms = new List<AccountDto>();

                vms.AddRange(r.Select(MapAccount));

                return vms;
            }
        }
    }
}

