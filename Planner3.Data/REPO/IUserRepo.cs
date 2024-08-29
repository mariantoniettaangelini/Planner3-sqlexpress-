using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IUserRepo
    {
        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByEmail(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
    }
}