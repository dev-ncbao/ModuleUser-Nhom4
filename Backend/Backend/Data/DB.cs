using Microsoft.EntityFrameworkCore;
using Backend.Entity;

namespace Backend.Data
{
    public class DB:DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }

    }
}
