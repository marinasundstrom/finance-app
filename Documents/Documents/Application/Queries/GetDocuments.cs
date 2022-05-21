using Documents.Infrastructure.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Documents.Application.Queries;

public record GetDocuments() : IRequest<IEnumerable<DocumentDto>>
{
    public class Handler : IRequestHandler<GetDocuments, IEnumerable<DocumentDto>>
    {
        private readonly DocumentsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(DocumentsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<DocumentDto>> Handle(GetDocuments request, CancellationToken cancellationToken)
        {
            var documents = await _context.Documents
                .OrderByDescending(x => x.Uploaded)
                .ToArrayAsync(cancellationToken);

            return documents.Select(document => new DocumentDto(document.Id, document.Title, GetUrl(document.BlobId), document.Uploaded));
        }

        private string GetUrl(string blobId)
        {
            var request = _httpContextAccessor.HttpContext!.Request;

            return $"{request.Scheme}://{request.Host}/documents/{blobId}";
        }
    }
}