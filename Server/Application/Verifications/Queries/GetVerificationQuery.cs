using System;

using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;

using MediatR;

using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Verifications.Shared;

namespace Accounting.Application.Verifications.Queries
{
    public class GetVerificationQuery : IRequest<VerificationDto>
    {
        public GetVerificationQuery(string verificationNo)
        {
            VerificationNo = verificationNo;
        }

        public string VerificationNo { get; }

        public class GetVerificationQueryHandler : IRequestHandler<GetVerificationQuery, VerificationDto>
        {
            private readonly IAccountingContext context;

            public GetVerificationQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<VerificationDto> Handle(GetVerificationQuery request, CancellationToken cancellationToken)
            {
                var v = await context.Verifications
                                 .Include(x => x.Entries)
                                 .OrderBy(x => x.Date)
                                 .AsNoTracking()
                                 .AsSplitQuery()
                                 .FirstOrDefaultAsync(x => x.VerificationNo == request.VerificationNo, cancellationToken);

                if (v is null) throw new Exception();

                return new VerificationDto
                {
                    VerificationNo = v.VerificationNo,
                    Date = v.Date,
                    Description = v.Description,
                    Debit = v.Entries.Sum(e => e.Debit.GetValueOrDefault()),
                    Credit = v.Entries.Sum(e => e.Credit.GetValueOrDefault()),
                    Attachment = GetAttachmentUrl(v.Attachment)
                };
            }
        }
    }
}