using ModuleUser.Entities;

namespace ModuleUser.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(string username);
        Task<User> Get(string username, string password);
        Task<User> Create(User user);
        Task Update(string username, User user);
        Task Delete(string username);
    }
}