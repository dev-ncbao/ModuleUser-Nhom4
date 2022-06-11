using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ModuleUser.Repositories;
using ModuleUser.Dtos;
using ModuleUser.Extensions;
using ModuleUser.Entities;
using System.Security.Cryptography;
using ModuleUser.Utils;

namespace ModuleUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository repository;
        private readonly EncryptorUtil encryptorUtil;

        public UsersController(IUsersRepository repository, EncryptorUtil encryptorUtil)
        {
            this.repository = repository;
            this.encryptorUtil = encryptorUtil;
        }

        // Login, Logout
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDto userDto)
        {
            var user = await repository.Get(userDto.Username, userDto.Password);

            if (user is null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("Logout/{username}")]
        public async Task<ActionResult> Logout(string username)
        {
            var user = await repository.Get(username);

            if (user is null)
            {
                return NotFound();
            }

            user.Expire = null;
            await repository.Update(username, user);
            return Ok();
        }
        //

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = (await repository.GetAll()).Select(u => u.AsDto());
            return users;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserDto>> Get(string username)
        {
            var user = await repository.Get(username);

            if (user is null)
            {
                return NotFound();
            }

            return user.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserCreationDto userDto)
        {
            var existingUser = await repository.Get(userDto.Username);

            if (existingUser != null)
            {
                return Conflict();
            }

            User user = new()
            {
                Username = userDto.Username,
                Password = encryptorUtil.Hash(userDto.Password),
                Name = userDto.Name
            };

            await repository.Create(user);

            return CreatedAtAction(nameof(Get), new { username = user.Username }, user.AsDto());
        }

        [HttpPut("{username}")]
        public async Task<ActionResult> Update(string username, UserUpdateDto userDto)
        {
            var existingUser = await repository.Get(username);

            if (existingUser is null)
            {
                return NotFound();
            }

            existingUser.Password = userDto.Password;
            existingUser.Name = userDto.Name;

            await repository.Update(username, existingUser);
            return NoContent();
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> Delete(string username)
        {
            var existingUser = await repository.Get(username);

            if (existingUser is null)
            {
                return NotFound();
            }

            await repository.Delete(username);
            return NoContent();
        }
    }
}