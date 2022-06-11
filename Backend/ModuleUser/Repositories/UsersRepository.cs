using Microsoft.EntityFrameworkCore;
using ModuleUser.Entities;

namespace ModuleUser.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext dbContext;

        public UsersRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> Create(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task Delete(string username)
        {
            var user = await dbContext.Users.FindAsync(username);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> Get(string username, string password)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }


        public async Task<User> Get(string username)
        {
            var user = await dbContext.Users.FindAsync(username);
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await dbContext.Users.ToListAsync();
            return users;
        }

        public async Task Update(string username, User user)
        {
            var existingUser = await dbContext.Users.FindAsync(username);
            existingUser.Name = user.Name;
            existingUser.Password = user.Password;
            existingUser.Expire = user.Expire;
            dbContext.Users.Update(existingUser);
            await dbContext.SaveChangesAsync();
        }
    }
}