using ProjectManagement.Data.Models;
using System.Data.Entity;

namespace ProjectManagement.Data.Services
{
    public class ProjectDbContext: DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Solution> Solutions { get; set; }
    }
}
