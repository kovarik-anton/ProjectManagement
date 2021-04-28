using ProjectManagement.Data.Models;
using ProjectManagement.Data.Services;
using ProjectManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository repository;

        public ProjectsController(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            var results = AutoMapper.Mapper.Map<IEnumerable<ProjectViewModel>>(repository.GetAllProjects());
            return View(results);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectViewModel project)
        {
            if (project.To < project.From)
            {
                ModelState.AddModelError("To", "To must be greater than From ");
            }

            if (ModelState.IsValid)
            {
                project.Solutions = new List<SolutionViewModel>();
                repository.AddProject(AutoMapper.Mapper.Map<Project>(project));
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var project = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(id));
            if (project == null)
            {
                return View("NotFound");
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectViewModel project)
        {
            var p = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(project.Id));
            project.Solutions = p.Solutions;

            if (project.To < project.From)
            {
                ModelState.AddModelError("To", "To must be greater than From ");
                return View();
            }

            if (!project.CheckIfDateRangeIsValid())
            {
                ModelState.AddModelError("From", "Some solutions date range will be out of project realization date");
                ModelState.AddModelError("To", "Some solutions date range will be out of project realization date");
                return View();
            }

            if (!project.CheckIfHoursAreValid())
            {
                ModelState.AddModelError("Hours", "Already more hours spent");
                return View();
            }

            if (ModelState.IsValid)
            {
                repository.EditProject(AutoMapper.Mapper.Map<Project>(project));
                return RedirectToAction("Index");
            }

            return View(project);
        }

        [HttpGet]
        public ActionResult Details(int id, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StateSortParm = sortOrder == "state" ? "state_desc" : "state";

            var project = AutoMapper.Mapper.Map<ProjectViewModel>(repository.GetProject(id));
            if (project == null)
            {
                return View("NotFound");
            }

            switch (sortOrder)
            {
                case "name_desc":
                    project.Solutions = project.Solutions.OrderByDescending(s => s.Name);
                    break;
                case "state":
                    project.Solutions = project.Solutions.OrderBy(s => s.State);
                    break;
                case "state_desc":
                    project.Solutions = project.Solutions.OrderByDescending(s => s.State);
                    break;
                default:
                    project.Solutions = project.Solutions.OrderBy(s => s.Name);
                    break;
            }

            return View(project);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            repository.DeleteProject(id);
            return RedirectToAction("Index");
        }
    }
}