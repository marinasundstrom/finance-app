using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Server.Controllers
{
    [Route("api/[controller]")]
    public class VerificationsController : Controller
    {
        private readonly AccountingContext context;

        public VerificationsController(AccountingContext context)
        {
            this.context = context;
        }

        // GET: api/values
        [HttpGet]
        public async Task<VerificationsResult> GetVerificationsAsync(int page = 0, int pageSize = 10)
        {
            var query = context.Verifications
                 .Include(x => x.Entries)
                 .OrderBy(x => x.Date)
                 .AsNoTracking()
                 .AsSplitQuery()
                 .AsQueryable();

            var totalItems = await query.CountAsync();

            var r = await query
               .Skip(pageSize * page)
               .Take(pageSize)
               .ToListAsync();

            var vms = new List<Verification>();

            vms.AddRange(r.Select(v => new Verification
            {
                VerificationNo = v.VerificationNo,
                Date = v.Date,
                Description = v.Description,
                Debit = v.Entries.Sum(e => e.Debit.GetValueOrDefault()),
                Credit = v.Entries.Sum(e => e.Credit.GetValueOrDefault())
            }));

            return new VerificationsResult(vms, totalItems);
        }

        [HttpPost]
        public async Task<string> CreateVerification([FromBody] CreateVerification dto)
        {
            var verificationCount = await context.Verifications.CountAsync();

            var verification = new Data.Verification
            {
                VerificationNo = $"V{verificationCount + 1}",
                Description = dto.Description,
                Date = DateTime.Now,
                Attachment = String.Empty
            };

            foreach(var entryDto in dto.Entries)
            {
                if(entryDto.Debit is null && entryDto.Credit is null
                    && entryDto.Debit is not null && entryDto.Credit is not null)
                {
                    throw new Exception("Cannot set both Debit and Credit.");
                }

                verification.Entries.Add(new Data.Entry
                {
                    Date = verification.Date,
                    AccountNo = entryDto.AccountNo,
                    Description = entryDto.Description ?? String.Empty,
                    Debit = entryDto.Debit,
                    Credit = entryDto.Credit
                });
            }

            context.Verifications.Add(verification);

            await context.SaveChangesAsync();

            return verification.VerificationNo;
        }
    }

    public record VerificationsResult(IEnumerable<Verification> Verifications, int TotalItems);

    public class Verification
    {
        public string VerificationNo { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Description { get; set; } = null!;

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }

    public class VerificationShort
    {
        public string VerificationNo { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Description { get; set; } = null!;
    }

    public class CreateVerification
    {
        public string Description { get; set; } = null!;

        [Required]
        public List<CreateEntry> Entries { get; set; } = new List<CreateEntry>()!;
    }

    public class CreateEntry
    {
        [Required]
        public int AccountNo { get; set; }

        public string? Description { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }
    }
}

