namespace ModuleUser.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime? Expire { get; set; }
    }
}