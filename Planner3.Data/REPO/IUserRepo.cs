using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IUserRepo
    {
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUsersByIdAsync(int id);
        Task UpdateUserAsync(User user);
    }
}