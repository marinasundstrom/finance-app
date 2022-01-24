using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Entries;
using Accounting.Application.Entries.Queries;
using Accounting.Application.Verifications;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Accounting.Controllers
{
    [Route("[controller]")]
    public class EntriesController : Controller
    {
        private readonly IMediator mediator;

        public EntriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<EntriesResult> GetEntriesAsync(int? accountNo = null, string? verificationNo = null, int page = 0, int pageSize = 10, ResultDirection direction = ResultDirection.Asc, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(new GetEntriesQuery(accountNo, verificationNo, page, pageSize, direction), cancellationToken);
        }
    }
}