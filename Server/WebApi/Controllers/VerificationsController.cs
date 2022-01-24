using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;
using Accounting.Application.Verifications.Commands;
using Accounting.Application.Verifications.Queries;

using Azure.Storage.Blobs;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Verifications.Shared;

namespace Accounting.Controllers
{
    [Route("[controller]")]
    public class VerificationsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAccountingContext context;
        private readonly BlobServiceClient blobServiceClient;

        public VerificationsController(IMediator mediator, IAccountingContext context, BlobServiceClient blobServiceClient)
        {
            this.mediator = mediator;
            this.context = context;
            this.blobServiceClient = blobServiceClient;
        }

        // GET: api/values
        [HttpGet]
        public async Task<VerificationsResult> GetVerificationsAsync(int page = 0, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(new GetVerificationsQuery(page, pageSize), cancellationToken);
        }

        [HttpGet("{verificationNo}")]
        [ProducesResponseType(typeof(VerificationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VerificationDto>> GetVerificationAsync(string verificationNo, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetVerificationQuery(verificationNo), cancellationToken);
        }

        [HttpPost]
        public async Task<string> CreateVerification([FromBody] CreateVerification dto, CancellationToken cancellationToken)
        {
            return await mediator.Send(new CreateVerificationCommand(dto.Description, dto.Entries), cancellationToken);
        }

        [HttpPost("/{verificationNo}/Attachment")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string?>> AddFileAttachmentToVerification(string verificationNo, IFormFile file, CancellationToken cancellationToken)
        {
            return await mediator.Send(new AddFileAttachmentToVerificationCommand(verificationNo, file.FileName, file.OpenReadStream()), cancellationToken);
        }
    }
}