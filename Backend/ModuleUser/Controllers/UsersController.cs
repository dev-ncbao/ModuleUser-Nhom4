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
        public async Task<IEnumerable<User>> GetAll()
        {
            return await dbcontext.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User account)
        {
            await dbcontext.Users.AddAsync(account);
            await dbcontext.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(User account)
        {
            var acc = await dbcontext.Users.Where(a => a.Username == account.Username &&
                a.Password == account.Password).SingleOrDefaultAsync();
            if (acc == null)
                return NotFound();
            return Ok();
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await dbcontext.Users.FindAsync(username);
            if (user == null)
                return BadRequest("Not found!");
            dbcontext.Users.Remove(user);
            await dbcontext.SaveChangesAsync();
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
            await dbcontext.SaveChangesAsync();
            return Ok(acc);
        }
    }
}