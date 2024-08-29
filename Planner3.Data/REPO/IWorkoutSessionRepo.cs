using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IWorkoutSessionRepo
    {
        Task<WorkoutSession> CreateSessionAsync(WorkoutSession workoutSession);
        Task DeleteSessionAsync(WorkoutSession workoutSession);
        Task<WorkoutSession> GetSessionById(int id);
        Task<IEnumerable<WorkoutSession>> GetSessionsAsync();
        Task UpdateSessionAsync(WorkoutSession workoutSession);
        Task<bool> UserExists(int userId);
    }
}