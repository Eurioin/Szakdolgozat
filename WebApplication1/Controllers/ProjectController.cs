using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly ProjectService projectService;

        public ProjectController(ProjectService service)
        {
            this.projectService = service;
        }

        [HttpGet]
        public ActionResult<Project> Details(string projectId)
        {
            return this.projectService.GetById(projectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(Project p)
        {
            this.projectService.Create(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Edit(string id, Project p)
        {
            this.projectService.Update(id, p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Delete(string projectId)
        {
            this.projectService.Remove(projectId);
        }
    }
}
