using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Documents.Application.Commands;
using Documents.Contracts;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Documents.Controllers;

[Route("[controller]")]
public class DocumentsController : Controller
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("UploadDocument")]
    public async Task UploadDocument(string title, IFormFile file)
    {
        await _mediator.Send(new UploadDocument(title, file.OpenReadStream()));
    }

    [HttpPost("GenerateDocument")]
    public async Task<IActionResult> GenerateDocument(string templateId, [FromBody] string model)
    {
        DocumentFormat documentFormat = DocumentFormat.Html;

        var stream = await _mediator.Send(new GenerateDocument(templateId, model));
        return File(stream, documentFormat == DocumentFormat.Html ? "application/html" : "application/pdf");
    }
}