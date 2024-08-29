using Planner3.Data.MODELS;

namespace Planner3.Data.REPO
{
    public interface IExerciseRepo
    {
        Task<Exercise> CreateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(Exercise exercise);
        Task<IEnumerable<Exercise>> GetExercisesAsync();
        Task<Exercise> GetExercisesByIdAsync(int id);
        Task UpdateExerciseAsync(Exercise exercise);
    }
}