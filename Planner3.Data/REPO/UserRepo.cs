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
    public class UserRepo : IUserRepo
    {
        private readonly PlannerContext _ctx;
        public UserRepo(PlannerContext ctx)
        {
            _ctx = ctx;
        }

        // per la registrazione ->
        public async Task<User> CreateUserAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _ctx.Users.FindAsync(id);
        }

        // per il login ->
        public async Task<User> GetUserByEmail(string email, string password)
        {
            return await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _ctx.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Weight = user.Weight;
                existingUser.Height = user.Height;
                existingUser.ExperienceLevel = user.ExperienceLevel;
                existingUser.Goals = user.Goals;
                existingUser.Name = user.Name;

                _ctx.Users.Update(existingUser);
                await _ctx.SaveChangesAsync();
            }
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _ctx.Users.FindAsync(id);
            if (user != null)
            {
                _ctx.Users.Remove(user);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
