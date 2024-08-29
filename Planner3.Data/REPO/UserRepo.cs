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
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _ctx.Users.ToListAsync();
            return users;
        }
        public async Task<User> GetUsersByIdAsync(int id)
        {
            return await _ctx.Users.FindAsync(id);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUserAsync(User user)
        {
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(User user)
        {
            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
        }
    }
}
