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
        public async Task<ActionResult<User>> Create(UserCreation account)
        {
            var existingUser = await dbcontext.Users.SingleOrDefaultAsync(u => u.Username == account.Username);

            if (existingUser != null)
            {
                return Conflict();
            }

            var user = new User()
            {
                Username = account.Username,
                Password = account.Password,
                Name = account.Name,
                Expire = null
            };

            await dbcontext.Users.AddAsync(user);
            await dbcontext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<User> GetUser(string username)
        {
            return dbcontext.Users.SingleOrDefault(x => x.Username == username);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLogin account)
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
                return BadRequest();
            dbcontext.Users.Remove(user);
            await dbcontext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put(User requests)
        {
            var acc = await dbcontext.Users.FindAsync(requests.Username);
            if (acc == null)
                return NotFound();
            acc.Name = requests.Name;
            acc.Password = requests.Password;
            acc.Expire = requests.Expire;
            await dbcontext.SaveChangesAsync();
            return Ok();
        }
    }
}