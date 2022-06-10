using Microsoft.EntityFrameworkCore;
using ModuleUser.Entities;

namespace ModuleUser.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base (options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
