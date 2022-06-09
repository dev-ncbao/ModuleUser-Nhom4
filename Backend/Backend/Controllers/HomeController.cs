using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Entity;

namespace Backend.Controllers
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
        [HttpPost("Login")]
        public async Task<ActionResult<Account>> Login(Account account)
        {
            var acc = dbcontext.Accounts.Where(a => a.username == account.username &&
                a.password == account.password).SingleOrDefault();
            if (acc == null)
                return NotFound();
            return Ok();
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
        public async Task<ActionResult<IEnumerable<Account>>> DeleteUser(string username)
        {
            var user = await dbcontext.Accounts.FindAsync(username);
            if (user == null)
                return BadRequest("Not found!");
            dbcontext.Accounts.Remove(user);
            dbcontext.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<Account>> Put(Account repuests)
        {
            var acc = await dbcontext.Accounts.FindAsync(repuests.username);
            if (acc == null)
                return NotFound();
            acc.username = repuests.username;
            acc.password = repuests.password;
            dbcontext.SaveChanges();
            return Ok(acc);

        }
    }
}