using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public async Task<EntriesResult> GetEntriesAsync(int? accountNo = null, string? verificationNo = null, int page = 0, int pageSize = 10, ResultDirection direction = ResultDirection.Asc)
        {
            var query = context.Entries
                        .Include(e => e.Verification)
                        .Include(e => e.Account)
                        .AsNoTracking()
                        .AsQueryable();

            if(direction == ResultDirection.Asc)
            {
                query = query.OrderBy(e => e.Date);
            }
            else
            {
                query = query.OrderByDescending(e => e.Date);
            }

            if (accountNo is not null)
            {
                query = query.Where(e => e.AccountNo == accountNo);
            }

            if (verificationNo is not null)
            {
                query = query.Where(e => e.VerificationNo == verificationNo);
            }

            var totalItems = await query.CountAsync();

            var entries = await query
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToListAsync();

            var entries2 = entries.Select(e => new Entry(
                e.Id,
                e.Date,
                new VerificationShort
                {
                    VerificationNo = e.VerificationNo,
                    Date = e.Verification.Date,
                    Description = e.Verification.Description,
                },
                new AccountShort
                {
                    AccountNo = e.AccountNo,
                    Name = e.Account.Name
                },
                e.Description,
                e.Debit,
                e.Credit
            ));

            return new EntriesResult(entries2, totalItems);
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultDirection
    {
        Desc,
        Asc
    }

    public record EntriesResult(IEnumerable<Entry> Entries, int TotalItems);

    public record Entry(int Id, DateTime Date, VerificationShort Verification, AccountShort Account,
        string Description, decimal? Debit, decimal? Credit);

}

