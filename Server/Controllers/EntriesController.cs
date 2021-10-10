using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Server.Controllers
{
    [Route("api/[controller]")]
    public class EntriesController : Controller
    {
        private readonly AccountingContext context;

        public EntriesController(AccountingContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Entry>> GetAsync(string? verificationNo = null)
        {
            var query = context.Entries
                        .Include(e => e.Verification)
                        .Include(e => e.Account)
                        .AsQueryable();


            if (verificationNo is not null)
            {
                query = query.Where(e => e.VerificationNo == verificationNo);
            }

            var entries = await query
                .AsNoTracking()
                .ToListAsync();

            return entries.Select(e => new Entry
            {
                Id = e.Id,
                Verification = new VerificationShort
                {
                    VerificationNo = e.VerificationNo,
                    Date = e.Verification.Date,
                    Description = e.Verification.Description,
                },
                Account = new AccountShort
                {
                    AccountNo = e.AccountNo,
                    Name = e.Account.Name
                },
                Description = e.Description,
                Debit = e.Debit,
                Credit = e.Credit
            });
        }
    }

    public class Entry
    {
        public int Id { get; set; }

        public VerificationShort Verification { get; set; } = null!;

        public AccountShort Account { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }
    }
}

