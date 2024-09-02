using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.MODELS;
using System.Security.Claims;

namespace Planner3.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PlannerContext _ctx;
        public UserController(PlannerContext ctx)
        {
            _ctx = ctx;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _ctx.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Restituisci l'oggetto utente completo
                    return Ok(new
                    {
                        id = user.Id,
                        name = user.Name,
                        email = user.Email,
                        experienceLevel = user.ExperienceLevel, // Assicurati che questi campi esistano nel modello utente
                        goals = user.Goals
                        // Aggiungi altri campi necessari
                    });
                }
                return Unauthorized(new
                {
                    message = "Email o password non corretti"
                });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_ctx.Users.Any(u => u.Email == model.Email))
                {
                    return Conflict(new
                    {
                        message = "è già presente un account con questa email"
                    });
                }

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Gender = model.Gender,
                    Height = model.Height,
                    Weight = model.Weight,
                    ExperienceLevel = model.ExperienceLevel,
                    Goals = model.Goals
                };

                _ctx.Users.Add(user);
                await _ctx.SaveChangesAsync();

                // auto login post registrazione ->
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Ok(new
                {
                    message = "registrazione avvenuta con successo"
                });
            }

            return BadRequest(ModelState);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new
            {
                message = "logout effettuato"
            });
        }
        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _ctx.Users
                //.Include(u=>u.Progresses)
                //.ThenInclude(p=>p.WorkoutSession)
                .FirstOrDefaultAsync(u=>u.Id == int.Parse(userId));

            if(user == null)
            {
                return NotFound("utente non trovato");
            }
            return Ok(user);
        }
        [HttpPost("Progress")] //modifica in una GET
        [Authorize]
        public async Task<IActionResult> SaveProgress(int workoutSessionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var workoutSession = await _ctx.WorkoutSessions.FindAsync(workoutSessionId);
            if (workoutSession == null)
            {
                return NotFound("Sessione di allenamento non trovata");
            }

            var progress = new Progress
            {
                UserId = userId,
                WorkoutSessionId = workoutSessionId,
                IsCompleted = true
            };

            _ctx.Progresses.Add(progress);
            await _ctx.SaveChangesAsync();

            return Ok(progress);
        }
    }
}
