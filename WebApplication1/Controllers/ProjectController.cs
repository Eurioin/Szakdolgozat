﻿using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Szakdolgozat.Models.DTOModels;
using System;
using System.IO;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly AccountService accountService;
        private readonly ProjectService projectService;
        private readonly TaskService taskService;
        private readonly SubTaskService subTaskService;

        public ProjectController(ProjectService service, AccountService srv, TaskService taskService, SubTaskService subTaskService)
        {
            this.projectService = service;
            this.accountService = srv;
            this.taskService = taskService;
            this.subTaskService = subTaskService;
        }


        [HttpPost("get")]
        public ActionResult<IEnumerable<Project>> GetAll([FromBody] Account usr)
        {
            var user = this.accountService.GetAll().Where(a => a.Username.Equals(usr.Username)).FirstOrDefault();
            var roles = user.UniqueRoles.Where(r => r.ToUpper().Equals("ADMIN"));
            if (roles.Count() != 0)
            {
                return this.projectService.GetAll();
            }
            var projects = new List<Project>();

            foreach (var project in user.AssignedProjects)
            {
                var p = this.projectService.GetById(project);
                if (p != null)
                {
                    projects.Add(p);
                }
            }

            return projects;
        }

        [HttpGet("project")]
        public ActionResult<Project> Details(string id)
        {
            return this.projectService.GetById(id);
        }

        [HttpPost("create")]
        public Project Create([FromBody] FromAngularProject p)
        {
            var project = new Project();
            project.Name = p.name;
            project.Assignees = new List<string>();
            var date = DateTime.UtcNow;
            project.DateOfCreation = date;
            project.Tasks = new List<string>();
            this.projectService.Create(project);

            project = this.projectService.GetAll().Where(a => a.DateOfCreation.Date.Equals(date.Date) && a.DateOfCreation.Hour.Equals(date.Hour) && a.DateOfCreation.Minute.Equals(date.Minute) && a.DateOfCreation.Second.Equals(date.Second)).FirstOrDefault();

            if (project == null)
            {
                return null;
            }

            string[] passedUsers;

            if (p.users.Count(x => x == ';') == 1 && p.users.LastIndexOf(';') == p.users.Length - 1)
            {
                passedUsers = new string[1];
                passedUsers[0] = p.users.Substring(0, p.users.LastIndexOf(';'));
            }
            else
            {
                passedUsers = p.users.Split(';');
            }

            foreach (var user in passedUsers)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user);
                    if (account != null)
                    {
                        project.Assignees.Add(account.Id);
                        project.Assignees = project.Assignees.Distinct().ToList();
                        project.NumberOfAssignees = project.Assignees.Count();
                        if (account.AssignedProjects == null)
                        {
                            account.AssignedProjects = new List<string>();
                        }
                        account.AssignedProjects.Add(project.Id);
                        account.AssignedProjects = account.AssignedProjects.Distinct().ToList();
                        this.accountService.Update(account.Id, account);
                    }
                }
            }
            this.projectService.Update(project.Id, project);
            return project;
        }

        [HttpPost("update")]
        public Project Edit([FromBody] FromAngularProject p)
        {
            var project = this.projectService.GetById(p.id);
            project.Name = p.name;
            string[] passedUsers;

            if (p.users.Count(x => x == ';') == 1 && p.users.LastIndexOf(';') == p.users.Length - 1)
            {
                passedUsers = new string[1];
                passedUsers[0] = p.users.Substring(0, p.users.LastIndexOf(';'));
            }
            else
            {
                passedUsers = p.users.Split(';');
            }
            project.Assignees = new List<string>();
            foreach (var user in passedUsers)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user);
                    if (account != null)
                    {
                        project.Assignees.Add(account.Id);
                        project.Assignees = project.Assignees.Distinct().ToList();
                        project.NumberOfAssignees = project.Assignees.Count();
                        if (account.AssignedProjects == null)
                        {
                            account.AssignedProjects = new List<string>();
                        }
                        account.AssignedProjects.Add(project.Id);
                        account.AssignedProjects = account.AssignedProjects.Distinct().ToList();
                        this.accountService.Update(account.Id, account);
                    }
                }
            }
            this.projectService.Update(project.Id, project);
            return this.projectService.GetById(project.Id);
        }

        [HttpPost("remove")]
        public void Delete([FromBody] FromAngularProject project)
        {
            var proj = this.projectService.GetById(project.id);

            var connectedAccounts = proj.Assignees;
            var connectedTasks = proj.Tasks;
            foreach (var acc in connectedAccounts)
            {
                var account = this.accountService.GetById(acc);
                var a = account.AssignedProjects.Where(ap => ap.Equals(project.id)).FirstOrDefault();
                account.AssignedProjects.Remove(a);
                this.accountService.Update(account.Id, account);
            }

            foreach (var t in connectedTasks)
            {
                var task = this.taskService.GetById(t);
                var subtasks = task.SubTasksIds;
                foreach (var st in subtasks)
                {
                    this.subTaskService.Remove(st);
                }
                this.taskService.Remove(task.Id);
            }

            this.projectService.Remove(project.id);
        }
    }
}
