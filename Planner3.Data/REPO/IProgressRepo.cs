using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IProgressRepo
    {
        Task<Progress> CreateProgressAsync(Progress progress);
        Task DeleteProgressAsync(int id);
        Task<List<Progress>> GetAllProgressAsync();
        Task<Progress> GetProgressByIdAsync(int id);
        Task UpdateProgressAsync(Progress progress);
    }
}