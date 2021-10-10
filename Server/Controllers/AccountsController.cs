using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly AccountingContext context;

        public AccountsController(AccountingContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAsync(int? accountClass = null, bool? showUnusedAccounts = false)
        {
            var query = context.Accounts
            .Include(a => a.Entries)
            .AsNoTracking()
            .AsQueryable();

            if (!showUnusedAccounts.GetValueOrDefault())
            {
                query = query.Where(a => a.Entries.Any());
            }

            if (accountClass is not null)
            {
                query = query.Where(a => a.Class == (AccountClass)accountClass);
            }

            var r = await query.ToListAsync();

            var vms = new List<Account>();

            vms.AddRange(r.Select(a => new Account
            {
                AccountNo = a.AccountNo,
                Class = (int)a.Class,
                Name = a.Name,
                Description = a.Description,
                Balance =
                    a.Entries.Sum(e => e.Debit.GetValueOrDefault())
                    - a.Entries.Sum(e => e.Credit.GetValueOrDefault())
            }));

            return vms;
        }
    }
}

public class Account
{
    public int AccountNo { get; set; }

    public int Class { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Balance { get; set; }
}

public class AccountShort
{
    public int AccountNo { get; set; }

    public string Name { get; set; } = null!;
}


