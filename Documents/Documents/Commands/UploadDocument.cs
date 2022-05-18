using System;

using Documents.Data;
using Documents.Models;
using Documents.Services;

using MediatR;

namespace Documents.Commands;

public record UploadDocument(string Title, Stream Stream) : IRequest<DocumentDto>
{
    public class Handler : IRequestHandler<UploadDocument, DocumentDto>
    {
        private readonly DocumentsContext _context;
        private readonly IFileUploaderService _fileUploaderService;

        public Handler(DocumentsContext context, IFileUploaderService fileUploaderService)
        {
            _context = context;
            _fileUploaderService = fileUploaderService;
        }

        public async Task<DocumentDto> Handle(UploadDocument request, CancellationToken cancellationToken)
        {
            var document = new Documents.Models.Document() {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Uploaded = DateTime.Now
            };

            _context.Documents.Add(document);

            document.BlobId = $"document-{document.Id}";

            await _fileUploaderService.UploadFileAsync(document.BlobId, request.Stream, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new DocumentDto(document.Id, document.Title, document.BlobId, document.Uploaded);
        }
    }
}
