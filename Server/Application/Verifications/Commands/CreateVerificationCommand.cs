using System;

using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;

using MediatR;

using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Verifications.Shared;

namespace Accounting.Application.Verifications.Commands
{
    public class CreateVerificationCommand : IRequest<string>
    {
        public CreateVerificationCommand(string description, List<CreateEntry> entries)
        {
            Description = description;
            Entries = entries;
        }

        public string Description { get; set; } = null!;

        public List<CreateEntry> Entries { get; set; } = new List<CreateEntry>()!;

        public class CreateVerificationCommandHandler : IRequestHandler<CreateVerificationCommand, string>
        {
            private readonly IAccountingContext context;

            public CreateVerificationCommandHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<string> Handle(CreateVerificationCommand request, CancellationToken cancellationToken)
            {
                var verificationCount = await context.Verifications.CountAsync(cancellationToken);

                var verification = new Domain.Entities.Verification
                {
                    VerificationNo = $"V{verificationCount + 1}",
                    Description = request.Description,
                    Date = DateTime.Now,
                    Attachment = String.Empty
                };

                foreach (var entryDto in request.Entries)
                {
                    if (entryDto.Debit is null && entryDto.Credit is null
                        && entryDto.Debit is not null && entryDto.Credit is not null)
                    {
                        throw new Exception("Cannot set both Debit and Credit.");
                    }

                    verification.Entries.Add(new Domain.Entities.Entry
                    {
                        Date = verification.Date,
                        AccountNo = entryDto.AccountNo,
                        Description = entryDto.Description ?? String.Empty,
                        Debit = entryDto.Debit,
                        Credit = entryDto.Credit
                    });
                }

                context.Verifications.Add(verification);

                await context.SaveChangesAsync(cancellationToken);

                return verification.VerificationNo;
            }
        }
    }
}