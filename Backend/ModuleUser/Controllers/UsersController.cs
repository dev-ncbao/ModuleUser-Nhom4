using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModuleUser.Data;
using ModuleUser.Entities;
using System.Security.Cryptography;
using System.Text;
using System;

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
        public async Task<User> GetUser(string username)
        {
            return await dbcontext.Users.SingleOrDefaultAsync(x => x.Username == username);
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
            DateTime time = (DateTime)user.Expire;
            if (user.Expire == null || (time.ToUniversalTime() - DateTime.UtcNow).TotalSeconds < 0)
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
            string pass = this.Hash(requests.Password);
            acc.Name = requests.Name;
            acc.Password = pass;
            acc.Expire = requests.Expire;
            await dbcontext.SaveChangesAsync();
            return Ok();
        }
    }
}