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
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd(); 

        }
    }
}
