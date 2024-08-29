using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IExerciseRepo
    {
        Task<Exercise> CreateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(int id);
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<List<Exercise>> GetExercisesAsync();
        Task UpdateExerciseAsync(Exercise exercise);
    }
}