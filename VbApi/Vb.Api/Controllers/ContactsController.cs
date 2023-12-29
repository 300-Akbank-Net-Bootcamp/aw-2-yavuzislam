using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly VbDbContext dbContext;

        public ContactsController(VbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<List<Contact>> Get()
        {
            var contacts = await dbContext.Set<Contact>()
                .Include(x=>x.Customer)
                .ToListAsync();
            
            return contacts;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<Contact> Get(int id)
        {
            var contact = await dbContext.Set<Contact>()
                .Include(x=>x.Customer)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            return contact;
        }

        [HttpPost]
        public async Task Post([FromBody] Contact contact)
        {
            await dbContext.Set<Contact>().AddAsync(contact);
            await dbContext.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Contact contact)
        {
            var fromdb = await dbContext.Set<Contact>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.ContactType = contact.ContactType;
            fromdb.Information = contact.Information;
            
            await dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var fromdb = await dbContext.Set<Contact>().SingleOrDefaultAsync(x => x.Id == id);
            fromdb.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }
}