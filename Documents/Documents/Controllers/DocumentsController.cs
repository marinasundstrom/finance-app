using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Documents.Commands;

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
}

