using System.ComponentModel.DataAnnotations;

namespace ModuleUser.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime? Expire { get; set; }
    }
}
