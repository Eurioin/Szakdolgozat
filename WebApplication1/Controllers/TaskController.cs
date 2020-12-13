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
        private readonly CommentService commentService;


        public TaskController(TaskService tService, SubTaskService stService, ProjectService projectService, AccountService accountService, CommentService commentService)
        {
            this.taskService = tService;
            this.subTaskService = stService;
            this.projectService = projectService;
            this.accountService = accountService;
            this.commentService = commentService;
        }

        [HttpGet("get")]
        public ActionResult<FromAngularTask> Details(string id)
        {
            var task = this.taskService.GetById(id);
            var dto = new FromAngularTask();
            if (task != null)
            {
                var subs = this.subTaskService.GetByProperty("parenttaskid", task.Id);
                var comments = this.commentService.GetByProperty("task", task.Id);
                dto.users = new List<string>();
                foreach (var usern in task.HandledBy)
                {
                    var usr = this.accountService.GetById(usern);
                    dto.users.Add(usr.Username + " - " + usr.Email);
                }

                // kis csalás
                for (int i = 0; i < comments.Count; i++)
                {
                    var username = this.accountService.GetById(comments[i].AuthorId).Username;
                    comments[i].AuthorId = username;
                }

                dto.subTasks = new List<SubTask>();
                dto.subTasks.AddRange(subs);
                dto.comments = new List<Comment>();
                dto.comments.AddRange(comments);
                dto.priority = task.Priority;
                dto.name = task.Name;
                dto.project = task.Project;
                dto.status = task.Status;
                dto.type = task.Type;
                dto.id = task.Id;
                dto.endDate = task.EndDate;
            }
            
            return dto;
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

            foreach (var user in t.users)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user)[0];
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

            foreach (var sb in t.subTasks)
            {
                if (sb.Description != null && sb.Description.Trim() != string.Empty)
                {
                    var sub = new SubTask();
                    sub.Description = sb.Description;
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

            // id-khez
            task.Comments = new List<string>();

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

            task.HandledBy = new List<string>();
            foreach (var user in t.users)
            {
                if (user.Length > 0)
                {
                    var account = this.accountService.GetByProperty("username", user)[0];
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

            task.SubTasksIds = new List<string>();
            foreach (var sub in t.subTasks)
            {
                if (sub.Description != null &&sub.Description != string.Empty)
                {
                    var subtask = new SubTask();
                    subtask.Description = sub.Description;
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
            var comments = task.Comments;
            proj.NumberOfTasks--;
            proj.Tasks.RemoveAll(ta => ta.Equals(t.id));

            foreach (var item in task.SubTasksIds)
            {
                this.subTaskService.Remove(item);
            }

            foreach (var com in comments)
            {
                this.commentService.Remove(com);
            }

            this.projectService.Update(proj.Id, proj);

            this.taskService.Remove(t.id);
        }
    }
}
