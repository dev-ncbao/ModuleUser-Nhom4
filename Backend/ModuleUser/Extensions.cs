using ModuleUser.Dtos;
using ModuleUser.Entities;

namespace ModuleUser.Extensions
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new()
            {
                Username = user.Username,
                Name = user.Name,
                Password = user.Password,
                Expire = user.Expire
            };
        }
    }
}