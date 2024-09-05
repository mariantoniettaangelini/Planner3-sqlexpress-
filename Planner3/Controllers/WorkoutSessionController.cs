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
            try
            {
                var sessions = await _ctx.WorkoutSessions
                    .Include(ws => ws.Exercises)
                    .ToListAsync();

                Console.WriteLine($"Recuperate {sessions.Count} sessioni di workout");

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore interno del server: " + ex.Message);
                return StatusCode(500, "Errore interno del server");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutSessionById(int id)
        {
            var session = await _ctx.WorkoutSessions
                .Include(ws => ws.Exercises)
                .FirstOrDefaultAsync(ws => ws.Id == id);

            if (session == null)
            {
                return NotFound("Workout session non trovata");
            }

            return Ok(session);
        }

        [HttpGet("ByType/{type}")]
        public async Task<IActionResult> GetWorkoutSessionsByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return BadRequest("Inserisci il Tipo di esercizio che desideri");
            }

            try
            {
                var sessions = await _ctx.WorkoutSessions
                    .Include(ws => ws.Exercises)
                    .Where(ws => ws.Type == type)
                    .ToListAsync();

                if (!sessions.Any())
                {
                    return NotFound("Non ci sono allenamenti disponibili per il tipo specificato");
                }

                return Ok(sessions);
            }
            catch (Exception ex)
            {
                // Logga l'errore
                return StatusCode(500, "Errore interno del server durante il recupero delle sessioni di allenamento");
            }
        }

        [HttpGet("ByMuscleGroup/{muscleGroup}")]
        public async Task<IActionResult> GetWorkoutSessionsByMuscleGroup(string muscleGroup)
        {
            if (string.IsNullOrEmpty(muscleGroup))
            {
                return BadRequest("Inserisci il Gruppo muscolare che desideri allenare");
            }

            var sessions = await _ctx.WorkoutSessions
                .Include(ws => ws.Exercises)
                .Where(ws => ws.Exercises.Any(e => e.MuscleGroup == muscleGroup))
                .ToListAsync();

            if (!sessions.Any())
            {
                return NotFound("Non ci sono allenamenti disponibili");
            }

            return Ok(sessions);
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
