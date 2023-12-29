using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EftTransactionsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public EftTransactionsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<EftTransaction>> Get()
        {
            var eftTransactions = await dbContext.Set<EftTransaction>()
                .Include(x => x.Account)
                .ToListAsync();
            
            return eftTransactions;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<EftTransaction> Get(int id)
        {
            var eftTransaction = await dbContext.Set<EftTransaction>()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            return eftTransaction;
        }

        [HttpPost]
        public async Task Post([FromBody] EftTransaction eftTransaction)
        {
            await dbContext.Set<EftTransaction>().AddAsync(eftTransaction);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] EftTransaction eftTransaction)
        {
            var fromdb = await dbContext.Set<EftTransaction>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.ReferenceNumber = eftTransaction.ReferenceNumber;
            fromdb.TransactionDate = eftTransaction.TransactionDate;
            fromdb.Amount = eftTransaction.Amount;
            fromdb.Description = eftTransaction.Description;
            fromdb.SenderAccount = eftTransaction.SenderAccount;
            fromdb.SenderIban = eftTransaction.SenderIban;
            fromdb.SenderName = eftTransaction.SenderName;

            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await dbContext.Set<EftTransaction>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }
}