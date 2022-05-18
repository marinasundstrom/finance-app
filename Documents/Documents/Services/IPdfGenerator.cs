
namespace Documents.Services
{
    public interface IPdfGenerator
    {
        Task<Stream> GeneratePdfFromHtmlAsync(string html, Uri? baseUrlOrPath = null);
    }
}