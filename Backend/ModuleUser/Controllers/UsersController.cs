using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuleUser.Data;
using ModuleUser.Entities;
using System.Security.Cryptography;
using System.Text;

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

        private string Hash(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = MD5.HashData(bytes);
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
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
                Password = Hash(account.Password),
                Name = account.Name,
                Expire = null
            };

            await dbcontext.Users.AddAsync(user);
            await dbcontext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await dbcontext.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLogin account)
        {
            var acc = await dbcontext.Users.Where(a => a.Username == account.Username &&
                a.Password == Hash(account.Password)).SingleOrDefaultAsync();

            if (acc == null)
                return NotFound();

            acc.Expire = DateTime.UtcNow.AddHours(1);
            await dbcontext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await dbcontext.Users.FindAsync(username);
            if (user == null)
                return BadRequest();
            if (user.Expire == null || DateTime.UtcNow > user.Expire )
            {
                dbcontext.Users.Remove(user);
                await dbcontext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

        }

        [HttpPut]
        public async Task<ActionResult> Put(UserUpdate requests)
        {
            var acc = await dbcontext.Users.FindAsync(requests.Username);
            if (acc == null)
                return NotFound();
            if (acc.Password != requests.Password)
            {
                acc.Password = Hash(requests.Password);
            }
            acc.Name = requests.Name;
            await dbcontext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("LogOut/{username}")]
        public async Task<ActionResult> LogOut(string username)
        {
            var acc = await dbcontext.Users.SingleOrDefaultAsync(a => a.Username == username);
            if (acc == null)
                return BadRequest();
            acc.Expire = null;
            dbcontext.SaveChanges();
            return Ok();
        }

        [HttpGet("Check/{username}")]
        public async Task<ActionResult> Check(string username)
        {
            var acc = await dbcontext.Users.SingleOrDefaultAsync(a => a.Username == username);

            if (acc.Expire == null || DateTime.UtcNow > acc.Expire)
            {
                return Unauthorized();
            }
            else
            {
                return Ok();
            }
        }
    }
}