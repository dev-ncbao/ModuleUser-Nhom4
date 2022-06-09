using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Entity;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly DB dbcontext;
        public HomeController(DB dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Get()
        {
            return await dbcontext.Accounts.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Account>> Create(Account account)
        {
            var acc = account;
            dbcontext.Accounts.Add(acc);
            await dbcontext.SaveChangesAsync();
            return Ok(acc);
        }
        [HttpDelete("Delete")]
        public async Task<ActionResult<IEnumerable<Account>>> DeleteUser(int id)
        {
            var user = await dbcontext.Accounts.FindAsync(id);
            if (user == null)
                return BadRequest("Not found!");
            dbcontext.Accounts.Remove(user);
            dbcontext.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<Account>> Put(Account repuests)
        {
            var acc = await dbcontext.Accounts.FindAsync(repuests.ID);
            if (acc == null)
                return NotFound();
            acc.username = repuests.username;
            acc.password = repuests.password;
            dbcontext.SaveChanges();
            return Ok(acc);

        }
    }
}