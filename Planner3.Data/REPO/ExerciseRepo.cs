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
        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            _ctx.Exercises.Add(exercise);
            await _ctx.SaveChangesAsync();
            return exercise;
        }
        public async Task<List<Exercise>> GetExercisesAsync()
        {
            return await _ctx.Exercises
                .Include(e => e.WorkoutSessions)
                .ToListAsync();
        }
        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            return await _ctx.Exercises
                .Include(e => e.WorkoutSessions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            var existingExercise = await _ctx.Exercises.FindAsync(exercise.Id);
            if (existingExercise != null)
            {
                existingExercise.Name = exercise.Name;
                existingExercise.Description = exercise.Description;
                existingExercise.Type = exercise.Type;
                existingExercise.MuscleGroup = exercise.MuscleGroup;

                _ctx.Exercises.Update(existingExercise);
                await _ctx.SaveChangesAsync();
            }
        }
        public async Task DeleteExerciseAsync(int id)
        {
            var exercise = await _ctx.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _ctx.Exercises.Remove(exercise);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
