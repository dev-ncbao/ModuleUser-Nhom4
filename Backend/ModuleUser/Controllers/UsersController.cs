using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuleUser.Data;
using ModuleUser.Entities;

namespace ModuleUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext dbcontext;
        public UsersController(UserDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await dbcontext.Users.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<User>> Create(User account)
        {
            var acc = account;
            dbcontext.Users.Add(acc);
            await dbcontext.SaveChangesAsync();
            return Ok(acc);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(User account)
        {
            var acc = dbcontext.Users.Where(a => a.Username == account.Username &&
                a.Password == account.Password).SingleOrDefault();
            if (acc == null)
                return NotFound();
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<User>>> DeleteUser(string username)
        {
            var user = await dbcontext.Users.FindAsync(username);
            if (user == null)
                return BadRequest("Not found!");
            dbcontext.Users.Remove(user);
            dbcontext.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<User>> Put(User requests)
        {
            var acc = await dbcontext.Users.FindAsync(requests.Username);
            if (acc == null)
                return NotFound();
            acc.Name = requests.Name;
            acc.Password = requests.Password;
            acc.Expire = requests.Expire;
            dbcontext.SaveChanges();
            return Ok(acc);

        }
    }
}