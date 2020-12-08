using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Szakdolgozat.Models.DTOModels;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly AccountService accountService;
        private readonly ProjectService projectService;

        public ProjectController(ProjectService service, AccountService srv)
        {
            this.projectService = service;
            this.accountService = srv;
        }


        [HttpPost("get")]
        public ActionResult<IEnumerable<Project>> GetAll([FromBody]Account usr)
        {
            var user = this.accountService.GetAll().Where(a => a.Username.Equals(usr.Username)).FirstOrDefault();
            if (user.Roles.Where(r=> r.ToUpper().Equals("ADMIN")) != null)
            {
                return this.projectService.GetAll();
            }
            return this.projectService.GetAll().Where(p => p.Assignees.Contains(user.Id)).ToList();
        }

        [HttpGet("project")]
        public ActionResult<Project> Details(string id)
        {
            return this.projectService.GetById(id);
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public void Create(Project p)
        {
            this.projectService.Create(p);
        }

        [HttpPost("update")]
        [ValidateAntiForgeryToken]
        public void Edit(string id, Project p)
        {
            this.projectService.Update(id, p);
        }

        [HttpPost("remove")]
        [ValidateAntiForgeryToken]
        public void Delete(string projectId)
        {
            this.projectService.Remove(projectId);
        }
    }
}
