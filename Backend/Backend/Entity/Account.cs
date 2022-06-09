using System.ComponentModel.DataAnnotations;

namespace Backend.Entity
{
    public class Account
    {   
        public string Name { get; set; } 
        [Key]
        public string username { get; set; }
        public string password { get; set; }
        public DateTime expire { get; set; }
    }
}
