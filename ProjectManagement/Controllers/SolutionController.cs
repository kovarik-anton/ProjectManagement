using ProjectManagement.Data.Models;
using ProjectManagement.Data.Services;
using ProjectManagement.ViewModels;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [RouteArea("projects/{projectId}")]
    [RoutePrefix("Solutions")]
    public class SolutionController : Controller
    {
        private readonly IProjectRepository repository;

        public SolutionController(IProjectRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Solution(int projectId, int solutionId)
        {
            var solution = AutoMapper.Mapper.Map<SolutionViewModel>(repository.GetSolution(projectId, solutionId));
            if (solution == null)
            {
                return View("NotFound");
            }

            return View(solution);
        }

        [HttpGet]
        public ActionResult Create(int projectId)
        {
            var project = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(projectId));
            if (project.State == Data.Data.Enums.State.Done)
            {
                return RedirectToAction("Details", "Projects", new { id = projectId});
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int projectId, SolutionViewModel solution)
        {
            if (solution.To < solution.From)
            {
                ModelState.AddModelError("To", "To must be greater than From ");
                return View();
            }

            var project = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(projectId));
            if (!project.DateInRange(solution.From, solution.To))
            {
                ModelState.AddModelError("From", "Solution date range is out of project realization date");
                ModelState.AddModelError("To", "Solution date range is out of project realization date");
                return View();
            }

            if (project.RemainingHours() < solution.Hours())
            {
                ModelState.AddModelError("From", "Solution has more hours than project remaining hours");
                ModelState.AddModelError("To", "Solution has more hours than project remaining hours");
                return View();
            }

            if (ModelState.IsValid)
            {
                repository.AddSolution(AutoMapper.Mapper.Map<Solution>(solution), projectId);
                return RedirectToAction($"Details/{projectId}", "Projects");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int projectId, int solutionId)
        {
            var solution = AutoMapper.Mapper.Map<SolutionViewModel>(repository.GetSolution(projectId, solutionId));
            if (solution == null)
            {
                return View("NotFound");
            }

            return View(solution);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int projectId, SolutionViewModel solution)
        {
            if (solution.To < solution.From)
            {
                ModelState.AddModelError("To", "To must be greater than From ");
                return View();
            }

            var project = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(projectId));
            if (!project.DateInRange(solution.From, solution.To))
            {
                ModelState.AddModelError("From", "Solution date range is out of project realization date");
                ModelState.AddModelError("To", "Solution date range is out of project realization date");
                return View();
            }

            if (project.RemainingHours(solution.Id) < solution.Hours())
            {
                ModelState.AddModelError("From", "Solution has more hours than project remaining hours");
                ModelState.AddModelError("To", "Solution has more hours than project remaining hours");
                return View();
            }

            if (ModelState.IsValid)
            {
                repository.EditSolution(projectId, AutoMapper.Mapper.Map<Solution>(solution));
                return RedirectToAction($"Details/{projectId}", "Projects");
            }

            return View(solution);
        }

        public ActionResult Details(int projectId, int solutionId)
        {
            var solution = AutoMapper.Mapper.Map<SolutionViewModel>(repository.GetSolution(projectId, solutionId));
            if (solution == null)
            {
                return View("NotFound");
            }

            return View(solution);
        }

        [HttpGet]
        public ActionResult Delete(int projectId, int solutionId)
        {
            repository.DeleteSolution(projectId, solutionId);
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }
    }
}