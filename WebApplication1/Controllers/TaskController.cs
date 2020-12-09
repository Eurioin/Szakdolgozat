using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;
using System;
using Szakdolgozat.Models.DTOModels;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskService taskService;
        private readonly SubTaskService subTaskService;
        private readonly ProjectService projectService;
        private readonly AccountService accountService;

        public TaskController(TaskService tService, SubTaskService stService, ProjectService projectService, AccountService accountService)
        {
            this.taskService = tService;
            this.subTaskService = stService;
            this.projectService = projectService;
            this.accountService = accountService;
        }

        [HttpGet("get")]
        public ActionResult<Task> Details(string id)
        {
            var task = this.taskService.GetById(id);
            if (task != null)
            {
                var subs = this.subTaskService.GetAll().Where(t => t.ParentTaksId.Equals(task.Id));
                task.ServerSideTaskList = new List<SubTask>();
                task.ServerSideTaskList.AddRange(subs);
            }
            return task;
        }

        [HttpPost("create")]
        public ActionResult<Task> Create([FromBody] FromAngularTask t)
        {
            var proj = this.projectService.GetById(t.project);
            var date = DateTime.UtcNow;

            var task = new Task();
            task.EndDate = t.endDate;
            task.Description = t.description;
            task.Name = t.name;
            task.Priority = t.priority;
            task.Status = t.status;
            task.Type = t.type;
            task.Project = t.project;
            task.HandledBy = new List<string>();
            task.SubTasksIds = new List<string>();
            task.Status = "Waiting for begin";
            task.DateOfCreation = date;
            this.taskService.Create(task);
            task = this.taskService.GetAll().Where(a => a.DateOfCreation.Date.Equals(date.Date) && a.DateOfCreation.Hour.Equals(date.Hour) && a.DateOfCreation.Minute.Equals(date.Minute) && a.DateOfCreation.Second.Equals(date.Second)).FirstOrDefault();
            string[] passedUsers;

            if (t.users.Count(x => x == ';') == 1 && t.users.LastIndexOf(';') == t.users.Length - 1)
            {
                passedUsers = new string[1];
                passedUsers[0] = t.users.Substring(0, t.users.LastIndexOf(';'));
            }
            else
            {
                passedUsers = t.users.Split(';');
            }

            foreach (var user in passedUsers)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user);
                    if (account != null)
                    {
                        proj.Assignees.Add(account.Id);
                        proj.Assignees = proj.Assignees.Distinct().ToList();
                        account.AssignedProjects.Add(proj.Id);
                        account.AssignedProjects = account.AssignedProjects.Distinct().ToList();
                        task.HandledBy.Add(account.Id);
                        task.HandledBy = task.HandledBy.Distinct().ToList();
                        this.accountService.Update(account.Id, account);
                    }
                }
            }

            string[] passedSubs;

            if (t.subTasks.Count(x => x == ';') == 1 && t.subTasks.LastIndexOf(';') == t.subTasks.Length - 1)
            {
                passedSubs = new string[1];
                passedSubs[0] = t.users.Substring(0, t.users.LastIndexOf(';'));
            }
            else
            {
                passedSubs = t.subTasks.Split(';');
            }

            foreach (var sb in passedSubs)
            {
                if (sb.Length > 0)
                {
                    var sub = new SubTask();
                    sub.Description = sb;
                    sub.ParentTaksId = task.Id;
                    sub.DateOfCreation = date;
                    this.subTaskService.Create(sub);
                    task.NumberOfSubTasks++;
                    sub = this.subTaskService.GetAll().Where(a => a.Description.Equals(sb) && a.DateOfCreation.Date.Equals(date.Date) && a.DateOfCreation.Hour.Equals(date.Hour) && a.DateOfCreation.Minute.Equals(date.Minute) && a.DateOfCreation.Second.Equals(date.Second)).FirstOrDefault();
                    task.SubTasksIds.Add(sub.Id);
                }
            }

            proj.NumberOfAssignees = proj.Assignees.Count();
            proj.NumberOfTasks++;
            proj.Tasks.Add(task.Id);

            this.projectService.Update(proj.Id, proj);
            this.taskService.Update(task.Id, task);
            return task;
        }

        [HttpPost("update")]
        public ActionResult<Task> Edit([FromBody] FromAngularTask t)
        {
            var proj = this.projectService.GetById(t.project);
            var date = DateTime.UtcNow;

            var task = this.taskService.GetById(t.id);
            task.Name = t.name;
            task.Priority = t.priority;
            task.Status = t.status;
            task.Type = t.type;
            task.Description = t.description;
            task.EndDate = t.endDate;

            string[] passedUsers;

            if (t.users.Count(x => x == ';') == 1 && t.users.LastIndexOf(';') == t.users.Length - 1)
            {
                passedUsers = new string[1];
                passedUsers[0] = t.users.Substring(0, t.users.LastIndexOf(';'));
            }
            else
            {
                passedUsers = t.users.Split(';');
            }
            task.HandledBy = new List<string>();
            foreach (var user in passedUsers)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user);
                    if (account != null)
                    {
                        proj.Assignees.Add(account.Id);
                        proj.Assignees = proj.Assignees.Distinct().ToList();
                        account.AssignedProjects.Add(proj.Id);
                        account.AssignedProjects = account.AssignedProjects.Distinct().ToList();
                        task.HandledBy.Add(account.Id);
                        task.HandledBy = task.HandledBy.Distinct().ToList();
                        this.accountService.Update(account.Id, account);
                    }
                }
            }

            foreach (var subt in task.SubTasksIds)
            {
                this.subTaskService.Remove(subt);
            }

            string[] subs;
            task.SubTasksIds = new List<string>();
            if (t.subTasks.Count(x => x == ';') == 1 && t.subTasks.LastIndexOf(';') == t.subTasks.Length - 1)
            {
                subs = new string[1];
                subs[0] = t.subTasks.Substring(0, t.subTasks.LastIndexOf(';'));
            }
            else
            {
                subs = t.subTasks.Split(';');
            }

            foreach (var sub in subs)
            {
                if (sub.Length > 0)
                {
                    var subtask = new SubTask();
                    subtask.Description = sub;
                    subtask.DateOfCreation = date;
                    subtask.ParentTaksId = task.Id;
                    this.subTaskService.Create(subtask);
                    subtask = this.subTaskService.GetAll().Where(a => a.Description.Equals(sub) && a.DateOfCreation.Date.Equals(date.Date) && a.DateOfCreation.Hour.Equals(date.Hour) && a.DateOfCreation.Minute.Equals(date.Minute) && a.DateOfCreation.Second.Equals(date.Second)).FirstOrDefault();
                    task.SubTasksIds.Add(subtask.Id);
                }
            }
            proj.NumberOfAssignees = proj.Assignees.Count();
            this.projectService.Update(proj.Id, proj);
            this.taskService.Update(task.Id, task);
            return this.taskService.GetById(task.Id);
        }

        [HttpPost("remove")]
        public void Delete([FromBody] FromAngularTask t)
        {
            var task = this.taskService.GetById(t.id);
            var proj = this.projectService.GetById(task.Project);
            proj.NumberOfTasks--;
            proj.Tasks.RemoveAll(ta => ta.Equals(t.id));
            foreach (var item in task.SubTasksIds)
            {
                this.subTaskService.Remove(item);
            }
            this.projectService.Update(proj.Id, proj);

            this.taskService.Remove(t.id);
        }
    }
}
