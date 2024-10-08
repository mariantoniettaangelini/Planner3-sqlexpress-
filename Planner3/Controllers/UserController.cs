﻿using Microsoft.AspNetCore.Authentication;
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
    [Authorize]
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

                    // l'oggetto utente completo
                    return Ok(new
                    {
                        id = user.Id,
                        name = user.Name,
                        email = user.Email,
                        experienceLevel = user.ExperienceLevel, 
                        goals = user.Goals
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
                .FirstOrDefaultAsync(u=>u.Id == int.Parse(userId));

            if(user == null)
            {
                return NotFound("utente non trovato");
            }
            return Ok(user);
        }
        [HttpGet("Progress")]
        [Authorize]
        public async Task<IActionResult> GetUserProgress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Identificazione utente non valida");
            }

            var progresses = await _ctx.Progresses
                .Where(p => p.UserId == int.Parse(userId))
                .Include(p => p.WorkoutSession) 
                .ToListAsync();

            if (!progresses.Any())
            {
                return NotFound("Non sono stati trovati progressi per l'utente.");
            }

            return Ok(progresses);
        }
    }
}
