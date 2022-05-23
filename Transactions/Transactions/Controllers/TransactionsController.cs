using Transactions.Application;
using Transactions.Domain.Enums;
using Transactions.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Invoices.Controllers;

[Route("[controller]")]
public class TransactionsController : ControllerBase 
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator) 
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<TransactionDto>>> GetTransactionsAsync(int page, int pageSize, [FromQuery] TransactionStatus[]? status,  [FromQuery] int? invoiceId) 
    {
        var result = await _mediator.Send(new GetTransactons(page, pageSize, status, invoiceId));
        return Ok(result);
    }
}