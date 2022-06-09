using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Entity
{
    public class Account
    {
        [Key]
        public string username { get; set; }
        public string password { get; set; }

    }
}
