using Invoices.Application;
using Invoices.Domain.Enums;
using Invoices.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers;

[Route("[controller]")]
public class InvoicesController : ControllerBase 
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator) 
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<InvoiceDto>>> GetInvoicesAsync(int page, int pageSize, [FromQuery] InvoiceStatus[]? status) 
    {
        var result = await _mediator.Send(new GetInvoices(page, pageSize, status));
        return Ok(result);
    }
}