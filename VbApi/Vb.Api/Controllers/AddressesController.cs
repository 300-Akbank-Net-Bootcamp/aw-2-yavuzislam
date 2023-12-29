using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public AddressesController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Address>> Get()
        {
            var addresses = await dbContext.Set<Address>()
                .Include(x=>x.Customer)
                .ToListAsync();
            
            return addresses;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<Address> Get(int id)
        {
            var address = await dbContext.Set<Address>()
                .Include(x=>x.Customer)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            return address;
        }

        [HttpPost]
        public async Task Post([FromBody] Address address)
        {
            await dbContext.Set<Address>().AddAsync(address);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Address address)
        {
            var fromdb = await dbContext.Set<Address>().SingleOrDefaultAsync(x=>x.Id == id);
            fromdb.Address1 = address.Address1;
            fromdb.Address2 = address.Address2;
            fromdb.City = address.City;
            fromdb.Country = address.Country;
            fromdb.County = address.County;
            fromdb.PostalCode = address.PostalCode;
            
            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb=await dbContext.Set<Address>().SingleOrDefaultAsync(x=>x.Id == id);
            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }
}