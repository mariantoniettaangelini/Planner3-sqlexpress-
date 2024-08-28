using Microsoft.EntityFrameworkCore;
using Planner3.Data.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.DATACONTEXT
{
    public class PlannerContext :DbContext
    {
        public PlannerContext (DbContextOptions<PlannerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
    }
}
