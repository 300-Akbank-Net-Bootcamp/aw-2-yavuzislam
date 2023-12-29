using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AccountTransactionsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountTransaction>> Get()
        {
            var accountTransactions = await dbContext.Set<AccountTransaction>()
                .Include(x => x.Account)
                .ToListAsync();
            
            return accountTransactions;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<AccountTransaction> Get(int id)
        {
            var accountTransaction = await dbContext.Set<AccountTransaction>()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            return accountTransaction;
        }

        [HttpPost]
        public async Task Post([FromBody] AccountTransaction accountTransaction)
        {
            await dbContext.Set<AccountTransaction>().AddAsync(accountTransaction);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] AccountTransaction accountTransaction)
        {
            var fromdb = await dbContext.Set<AccountTransaction>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.ReferenceNumber = accountTransaction.ReferenceNumber;
            fromdb.TransactionDate = accountTransaction.TransactionDate;
            fromdb.Amount = accountTransaction.Amount;
            fromdb.Description = accountTransaction.Description;
            fromdb.TransferType = accountTransaction.TransferType;

            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await dbContext.Set<AccountTransaction>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }
}