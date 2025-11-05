using DAFWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DAFWebApp.Data
{
    public class DAFDbContext : DbContext
    {
        public DAFDbContext(DbContextOptions<DAFDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerTask> VolunteerTasks { get; set; } 
    }
}
