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
    public class VerificationsController : Controller
    {
        private readonly AccountingContext context;

        public VerificationsController(AccountingContext context)
        {
            this.context = context;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Verification>> GetAsync()
        {
            var query = context.Verifications
                 .Include(x => x.Entries)
                 .AsQueryable();

            var r = await query
               .AsNoTracking()
               .AsSplitQuery()
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

            return vms;
        }
    }

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
}

