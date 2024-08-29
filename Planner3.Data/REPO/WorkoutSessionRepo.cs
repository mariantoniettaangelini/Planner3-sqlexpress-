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
    public class WorkoutSessionRepo : IWorkoutSessionRepo
    {
        private readonly PlannerContext _ctx;

        public WorkoutSessionRepo(PlannerContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<WorkoutSession>> GetSessionsAsync()
        {
            return await _ctx.WorkoutSessions.ToListAsync();
        }

        public async Task<WorkoutSession> GetSessionById(int id)
        {
            return await _ctx.WorkoutSessions.FindAsync(id);
        }

        public async Task<WorkoutSession> CreateSessionAsync(WorkoutSession workoutSession)
        {
            _ctx.WorkoutSessions.Add(workoutSession);
            await _ctx.SaveChangesAsync();
            return workoutSession;
        }

        public async Task UpdateSessionAsync(WorkoutSession workoutSession)
        {
            _ctx.WorkoutSessions.Update(workoutSession);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(WorkoutSession workoutSession)
        {
            _ctx.WorkoutSessions.Remove(workoutSession);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> UserExists(int userId)
        {
            return await _ctx.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
