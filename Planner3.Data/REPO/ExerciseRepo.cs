using Microsoft.EntityFrameworkCore;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.REPO
{
    public class ExerciseRepo : IExerciseRepo
    {
        private readonly PlannerContext _ctx;
        public ExerciseRepo(PlannerContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IEnumerable<Exercise>> GetExercisesAsync()
        {
            var exercises = await _ctx.Exercises.ToListAsync();
            return exercises;
        }
        public async Task<Exercise> GetExercisesByIdAsync(int id)
        {
            return await _ctx.Exercises.FindAsync(id);
        }
        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            _ctx.Exercises.Add(exercise);
            _ctx.SaveChanges();
            return exercise;
        }
        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            _ctx.Exercises.Update(exercise);
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteExerciseAsync(Exercise exercise)
        {
            _ctx.Exercises.Remove(exercise);
            await _ctx.SaveChangesAsync();
        }
    }
}
