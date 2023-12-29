using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AccountsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> Get()
        {
            var accounts = await dbContext.Set<Account>()
                .Include(x => x.Customer)
                .Include(x => x.AccountTransactions)
                .Include(x => x.EftTransactions)
                .ToListAsync();
            
            return accounts;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<Account> Get(int id)
        {
            var account = await dbContext.Set<Account>()
                .Include(x => x.Customer)
                .Include(x => x.AccountTransactions)
                .Include(x => x.EftTransactions)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            return account;
        }

        [HttpPost]
        public async Task Post([FromBody] Account account)
        {
            await dbContext.Set<Account>().AddAsync(account);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Account account)
        {
            var fromdb = await dbContext.Set<Account>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.AccountNumber = account.AccountNumber;
            fromdb.IBAN = account.IBAN;
            fromdb.Balance = account.Balance;
            fromdb.CurrencyType = account.CurrencyType;
            fromdb.Name = account.Name;
            fromdb.OpenDate = account.OpenDate;

            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await dbContext.Set<Account>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }
}