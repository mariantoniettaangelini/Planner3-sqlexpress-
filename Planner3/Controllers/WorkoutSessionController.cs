using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.MODELS;
using System.Security.Claims;

namespace Planner3.Controllers
{
    [Route("api/workoutsession")]
    [ApiController]
    [Authorize]
    public class WorkoutSessionController : ControllerBase
    {
        private readonly PlannerContext _ctx;
        public WorkoutSessionController(PlannerContext ctx)
        {
            _ctx = ctx;
        }
        [HttpGet]
        public async Task<IActionResult> GetWorkoutSessions()
        {
            var sessions = await _ctx.WorkoutSessions
                .Include(ws => ws.Exercises)
                .ToListAsync();

            return Ok(sessions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutSessionById(int id)
        {
            var session = await _ctx.WorkoutSessions
                .Include (ws => ws.Exercises)
                .FirstOrDefaultAsync(ws => ws.Id == id);
            if(session == null)
            {
                return NotFound("sessione di allenamento non trovata");
            }
            return Ok(session);
        }
        [HttpPost("Choose/{id}")]
        public async Task<IActionResult> ChooseSession(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var workoutSession = await _ctx.WorkoutSessions.FindAsync(id);
            if (workoutSession == null)
            {
                return NotFound("sessione di allenamento non trovata");
            }

            var progress = new Progress
            {
                UserId = userId,
                WorkoutSessionId = id,
                IsCompleted = true
            };

            _ctx.Progresses.Add(progress);
            await _ctx.SaveChangesAsync();

            return Ok(progress);
        }


    }
}
