using System;
using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Verifications.Shared;

namespace Accounting.Application.Verifications.Commands
{
	public class AddFileAttachmentToVerificationCommand : IRequest<string>
	{
        public AddFileAttachmentToVerificationCommand(string verificationNo, string name, Stream stream)
        {
            VerificationNo = verificationNo;
            Name = name;
            Stream = stream;
        }

        public string VerificationNo { get; }

        public string Name { get; }

        public Stream Stream { get; }

        public class AddFileAttachmentToVerificationCommandHandler : IRequestHandler<AddFileAttachmentToVerificationCommand, string>
        {
            private readonly IAccountingContext context;
            private readonly IBlobService blobService;

            public AddFileAttachmentToVerificationCommandHandler(IAccountingContext context, IBlobService blobService)
            {
                this.context = context;
                this.blobService = blobService;
            }

            public async Task<string> Handle(AddFileAttachmentToVerificationCommand request, CancellationToken cancellationToken)
            {
                var verification = await context.Verifications
                     .FirstAsync(x => x.VerificationNo == request.VerificationNo, cancellationToken);

                if (!string.IsNullOrEmpty(verification.Attachment))
                {
                    throw new Exception("There is already an attachment to this verification.");
                }

                var blobName = $"{request.VerificationNo}-{request.Name}";

                await blobService.UploadBloadAsync(blobName, request.Stream);

                verification.Attachment = blobName;

                await context.SaveChangesAsync(cancellationToken);

                return GetAttachmentUrl(verification.Attachment)!;
            }
        }
    }
}

