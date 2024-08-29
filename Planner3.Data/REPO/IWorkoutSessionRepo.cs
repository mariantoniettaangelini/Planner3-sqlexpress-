using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IWorkoutSessionRepo
    {
        Task<WorkoutSession> CreateSessionAsync(WorkoutSession session);
        Task DeleteSessionAsync(int id);
        Task<List<WorkoutSession>> GetAllSessionsAsync();
        Task<WorkoutSession> GetSessionsByIdAsync(int id);
        Task UpdateSessionAsync(WorkoutSession session);
    }
}