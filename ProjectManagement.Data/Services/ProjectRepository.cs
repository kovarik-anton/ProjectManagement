using ProjectManagement.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace ProjectManagement.Data.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectDbContext ctx;

        public ProjectRepository(ProjectDbContext ctx)
        {
            this.ctx = ctx;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return ctx.Projects;
        }

        public Project GetProject(int id)
        {
            return ctx.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Solutions)
                .FirstOrDefault();
        }

        public void AddProject(Project project)
        {
            ctx.Projects.Add(project);
            ctx.SaveChanges();
        }

        public void EditProject(Project project)
        {
            var p = ctx.Projects.Find(project.Id);
            ctx.Entry(p).CurrentValues.SetValues(project);
            ctx.SaveChanges();
        }

        public void DeleteProject(int projectId)
        {
            var project = GetProject(projectId);
            project.Solutions.Clear();
            ctx.SaveChanges();
            ctx.Projects.Remove(project);
            ctx.SaveChanges();
        }

        public Solution GetSolution(int projectId, int solutionId)
        {
            return ctx.Solutions
                .Where(s => s.Id == solutionId)
                .FirstOrDefault();
        }

        public void AddSolution(Solution solution, int projectId)
        {
            solution.Project = GetProject(projectId);
            ctx.Solutions.Add(solution);
            ctx.SaveChanges();
        }

        public void EditSolution(int projectId, Solution solution)
        {
            var s = ctx.Solutions.Find(solution.Id);
            ctx.Entry(s).CurrentValues.SetValues(solution);
            ctx.SaveChanges();
        }

        public void DeleteSolution(int projectId, int solutionId)
        {
            var solution = ctx.Solutions.Find(solutionId);
            ctx.Solutions.Remove(solution);
            ctx.SaveChanges();
        }

        public bool SaveAll()
        {
            return ctx.SaveChanges() > 0;
        }
    }
}
