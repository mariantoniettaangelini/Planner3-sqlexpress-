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
    public class ProgressRepo : IProgressRepo
    {
        private readonly PlannerContext _ctx;
        public ProgressRepo(PlannerContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Progress>> GetAllProgressAsync()
        {
            return await _ctx.Progresses
                .Include(p => p.User)
                .Include(p => p.WorkoutSession)
                .ToListAsync();
        }
        public async Task<Progress> GetProgressByIdAsync(int id)
        {
            return await _ctx.Progresses
                .Include(p => p.User)
                .Include(p => p.WorkoutSession)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Progress> CreateProgressAsync(Progress progress)
        {
            _ctx.Progresses.Add(progress);
            await _ctx.SaveChangesAsync();
            return progress;
        }
        public async Task DeleteProgressAsync(int id)
        {
            var progress = await _ctx.Progresses.FindAsync(id);
            if (progress != null)
            {
                _ctx.Progresses.Remove(progress);
                await _ctx.SaveChangesAsync();
            }
        }
        public async Task UpdateProgressAsync(Progress progress)
        {
            _ctx.Progresses.Update(progress);
            await _ctx.SaveChangesAsync();
        }
    }
}
