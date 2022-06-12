using Microsoft.AspNetCore.Cors;
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
            string pass = this.Hash(account.Password);
            var user = new User()
            {
                Username = account.Username,
                Password = pass,
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
            string pass = this.Hash(account.Password);
            var acc = await dbcontext.Users.Where(a => a.Username == account.Username &&
                a.Password == pass).SingleOrDefaultAsync();
            if (acc == null)
                return NotFound();
            TimeSpan aInterval = new System.TimeSpan(0, 1, 0, 0);
            acc.Expire = DateTime.Now.Add(aInterval);
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
            if(user.Expire == null)
            {
                dbcontext.Users.Remove(user);
                await dbcontext.SaveChangesAsync();
                return Ok();
            }
            DateTime time = (DateTime)user.Expire;
            if (user.Expire == null || (time.ToUniversalTime()-DateTime.UtcNow).TotalSeconds<0)
            {
                dbcontext.Users.Remove(user);
                await dbcontext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
            
        }

        [HttpPut]
        public async Task<ActionResult> Put(User requests)
        {
            var acc = await dbcontext.Users.FindAsync(requests.Username);
            if (acc == null)
                return NotFound();
            string pass = requests.Password;
            if (acc.Password != requests.Password)
            {
                pass = this.Hash(requests.Password);
            }
            acc.Name = requests.Name;
            acc.Password = pass;
            acc.Expire = requests.Expire;
            await dbcontext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("LogOut")]
        public async Task<ActionResult> LogOut(UserExpire user)
        {
            var acc = await dbcontext.Users.SingleOrDefaultAsync(a => a.Username == user.Username);
            if(acc == null)
                return BadRequest();
            acc.Expire = null;
            dbcontext.SaveChanges();
            return Ok();
        }
    }
}