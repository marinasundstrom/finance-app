using System;
using Accounting.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Accounts.Mappings;

namespace Accounting.Application.Accounts.Queries
{
	public class GetAccountQuery : IRequest<AccountDto>
	{
		public GetAccountQuery(int accountNo)
		{
            AccountNo = accountNo;
        }

        public int AccountNo { get; }

        public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
        {
            private readonly IAccountingContext context;

            public GetAccountQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
            {
                var account = await context.Accounts
                    .Include(a => a.Entries)
                    .AsNoTracking()
                    .AsQueryable()
                    .FirstAsync(a => a.AccountNo == request.AccountNo, cancellationToken);

                return MapAccount(account);
            }
        }
    }
}

