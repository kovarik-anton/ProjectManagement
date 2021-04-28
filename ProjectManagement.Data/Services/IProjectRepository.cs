using ProjectManagement.Data.Models;
using System.Collections.Generic;

namespace ProjectManagement.Data.Services
{
    public interface IProjectRepository
    { 
        IEnumerable<Project> GetAllProjects();
        Project GetProject(int projectId);
        void AddProject(Project project);
        void EditProject(Project project);
        void DeleteProject(int projectId);
        Solution GetSolution(int projectId, int solutionId);
        void AddSolution(Solution solution, int projectId);
        void EditSolution(int projectId, Solution solution);
        void DeleteSolution(int projectId, int solutionId);
        bool SaveAll();
    }
}