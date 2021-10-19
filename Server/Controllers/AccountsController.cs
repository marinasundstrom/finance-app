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
    public class AccountsController : Controller
    {
        private readonly AccountingContext context;

        public AccountsController(AccountingContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAccountsAsync(int? accountClass = null, bool? showUnusedAccounts = false)
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
                query = query.Where(a => a.Class == (Data.AccountClass)accountClass);
            }

            var r = await query.ToListAsync();

            var vms = new List<Account>();

            vms.AddRange(r.Select(MapAccount));

            return vms;
        }

        [HttpGet("{accountNo:int}")]
        public async Task<Account> GetAccountAsync(int accountNo)
        {
            var account = await context.Accounts
                .Include(a => a.Entries)
                .AsNoTracking()
                .AsQueryable()
                .FirstAsync(a => a.AccountNo == accountNo);

            return MapAccount(account);
        }

        private static Account MapAccount(Data.Account account)
        {
            return new Account
            {
                AccountNo = account.AccountNo,
                Class = new AccountClass
                {
                    Id = (int)account.Class,
                    Description = account.Class.GetAttribute<DisplayAttribute>()!.Name!
                },
                Name = account.Name,
                Description = account.Description,
                Balance =
                    account.Entries.Sum(e => e.Debit.GetValueOrDefault())
                    - account.Entries.Sum(e => e.Credit.GetValueOrDefault())
            };
        }

        [HttpGet("Classes")]
        public IEnumerable<AccountClass> GetAccountClassesAsync()
        {
            return Enum.GetValues<Data.AccountClass>().Select(ac =>
            {
                return new AccountClass
                {
                    Id = (int)ac,
                    Description = ac.GetAttribute<DisplayAttribute>()!.Name!
                };
            });
        }


        [HttpGet("Classes/Summary")]
        public async Task<IEnumerable<AccountClassSummary>> GetAccountClassSummaryAsync(int[] accountNo)
        {
            var query = context.Accounts
                .Include(a => a.Entries)
                .Where(a => a.Entries.Any())
                .AsNoTracking()
                .AsQueryable();

            var accounts = await query.ToListAsync();

            var classes = accounts.GroupBy(x => x.Class);

            return classes.Select(c =>
            {
                var name = c.Key.GetAttribute<DisplayAttribute>()!.Name!;
                return new AccountClassSummary(
                    (int)c.Key,
                    name,
                    c.Sum(a => a.Entries.Select(g => g.Debit.GetValueOrDefault() - g.Credit.GetValueOrDefault()).Sum())
                );
            });
        }

        [HttpGet("History")]
        public async Task<AccountBalanceHistory> GetAccountHistoryAsync(int[] accountNo)
        {
            List<(int Year, int Month)> months = new();

            DateTime startDate = DateTime.Today.Subtract(TimeSpan.FromDays(365));
            DateTime endDate = DateTime.Today;
            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddMonths(1))
            {
                months.Add((dt.Year, dt.Month));
            }

            var query = context.Accounts
                .Include(a => a.Entries)
                .ThenInclude(e => e.Verification)
                .Where(a => a.Entries.Any())
                .AsNoTracking()
                .AsQueryable();

            if (accountNo.Any())
            {
                query = query.Where(a => accountNo.Any(x => x == a.AccountNo));
            }

            var accounts = await query.ToListAsync();

            List<string> labels = months.Select(m => new DateOnly(m.Year, m.Month, 1).ToString("MMM yy")).ToList();
            Dictionary<Data.Account, List<decimal>> series = new();

            foreach (var month in months)
            {
                foreach(var account in accounts)
                {
                    if (!series.ContainsKey(account))
                    {
                        series[account] = new List<decimal>();
                    }

                    var entries = account.Entries.Where(x => x.Verification.Date.Year == month.Year && x.Verification.Date.Month == month.Month);
                    var sum = entries.Select(g => g.Debit.GetValueOrDefault() - g.Credit.GetValueOrDefault()).Sum();

                    if(sum < 0)
                    {
                        // Assume it is supposed to be a negative value but that we want to represent it as a positve value.
                        sum = sum * -1;
                    }

                    series[account].Add(sum);
                }
            }

            return new AccountBalanceHistory(
                labels.ToArray(),
                series.Select(asum => new AccountSeries(
                    $"{asum.Key.AccountNo} {asum.Key.Name}",
                    asum.Value)));
        }
    }

    public record class AccountClassSummary(int Id, string Name, decimal Balance);

    public record class AccountSummary(int AccountNo, string Name, decimal Balance);

    public record class AccountSeries(string Name, IEnumerable<decimal> Data);

    public record class AccountBalanceHistory(string[] Labels, IEnumerable<AccountSeries> Series);

    public class Account
    {
        public int AccountNo { get; set; }

        public AccountClass Class { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Balance { get; set; }
    }

    public class AccountShort
    {
        public int AccountNo { get; set; }

        public string Name { get; set; } = null!;
    }

    public class AccountClass
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;
    }
}