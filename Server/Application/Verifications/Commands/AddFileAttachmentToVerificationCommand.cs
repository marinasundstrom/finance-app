using Accounting.Application.Common.Interfaces;
using Accounting.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Shared;

namespace Accounting.Application.Verifications.Commands
{
    public class AddFileAttachmentToVerificationCommand : IRequest<string>
    {
        public AddFileAttachmentToVerificationCommand(int verificationId, string name, Stream stream)
        {
            VerificationId = verificationId;
            Name = name;
            Stream = stream;
        }

        public int VerificationId { get; }

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
                    .Include(v => v.Attachments)
                    .FirstAsync(x => x.Id == request.VerificationId, cancellationToken);

                if (verification.Attachments.Any())
                {
                    throw new Exception("There is already an attachment to this verification.");
                }

                var blobName = $"{request.VerificationId}-{request.Name}";

                await blobService.UploadBloadAsync(blobName, request.Stream);

                var attachment = new Attachment();

                attachment.Id = blobName;

                verification.Attachments.Add(attachment);

                await context.SaveChangesAsync(cancellationToken);

                return GetAttachmentUrl(attachment.Id)!;
            }
        }
    }
}