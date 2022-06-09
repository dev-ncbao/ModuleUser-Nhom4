using Microsoft.EntityFrameworkCore;
using WebApplication4.Entity;

namespace WebApplication4.Data
{
    public class DB:DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }

    }
}
