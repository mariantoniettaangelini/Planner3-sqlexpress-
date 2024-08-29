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
        public async Task<WorkoutSession> CreateSessionAsync(WorkoutSession session)
        {
            _ctx.WorkoutSessions.Add(session);
            await _ctx.SaveChangesAsync();
            return session;
        }
        public async Task<List<WorkoutSession>> GetAllSessionsAsync()
        {
            return await _ctx.WorkoutSessions
                .Include(s => s.Exercises)
                .ToListAsync();
        }
        public async Task<WorkoutSession> GetSessionsByIdAsync(int id)
        {
            return await _ctx.WorkoutSessions
                .Include(s => s.Exercises)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task UpdateSessionAsync(WorkoutSession session)
        {
            var existingSession = await _ctx.WorkoutSessions.FindAsync(session.Id);
            if (existingSession != null)
            {
                existingSession.Name = session.Name;
                existingSession.Description = session.Description;
                existingSession.Level = session.Level;
                existingSession.Duration = session.Duration;
                existingSession.Type = session.Type;

                _ctx.WorkoutSessions.Update(existingSession);
                await _ctx.SaveChangesAsync();
            }
        }
        public async Task DeleteSessionAsync(int id)
        {
            var session = await _ctx.WorkoutSessions.FindAsync(id);
            if (session != null)
            {
                _ctx.WorkoutSessions.Remove(session);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
