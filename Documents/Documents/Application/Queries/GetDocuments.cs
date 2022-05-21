using System.Linq;

using Documents.Application.Common.Models;
using Documents.Infrastructure.Persistence;

using MassTransit;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Documents.Application.Queries;

public record GetDocuments(int Page, int PageSize) : IRequest<ItemsResult<DocumentDto>>
{
    public class Handler : IRequestHandler<GetDocuments, ItemsResult<DocumentDto>>
    {
        private readonly DocumentsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(DocumentsContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ItemsResult<DocumentDto>> Handle(GetDocuments request, CancellationToken cancellationToken)
        {
            var query = _context.Documents
                .AsSplitQuery()
                .AsNoTracking()
                .OrderByDescending(x => x.Uploaded)
                .AsQueryable();

            int totalItems = await query.CountAsync(cancellationToken);

            query = query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize);

            var items = await query.ToArrayAsync(cancellationToken);

            return new ItemsResult<DocumentDto>(
                items.Select(document => new DocumentDto(document.Id, document.Title, GetUrl(document.BlobId), document.Uploaded)),
                totalItems);
        }

        private string GetUrl(string blobId)
        {
            var request = _httpContextAccessor.HttpContext!.Request;

            return $"{request.Scheme}://{request.Host}/documents/{blobId}";
        }
    }
}