using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Szakdolgozat.Models.DatabaseModels;
using Szakdolgozat.Services;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService commentService;
        private readonly TaskService taskService;
        private readonly AccountService accountService;

        public CommentController(CommentService commentService, TaskService taskService, AccountService accountService)
        {
            this.accountService = accountService;
            this.taskService = taskService;
            this.commentService = commentService;
        }

        [HttpPost("add")]
        public void SaveComment([FromBody]Comment comment)
        {
            var date = DateTime.UtcNow;
            // fordítsunk a nevezéseken, username jön, nekünk id kell, username pedig unique
            comment.AuthorId = this.accountService.GetByProperty("username", comment.AuthorId)[0].Id;
            comment.DateOfCreation = date;
            this.commentService.Create(comment);
            comment = this.commentService.GetByProperty("task", comment.Task).Where(c => c.AuthorId == comment.AuthorId && c.DateOfCreation.Year.Equals(date.Year) && c.DateOfCreation.Month.Equals(date.Month) && c.DateOfCreation.Day.Equals(date.Day) && c.DateOfCreation.Hour.Equals(date.Hour) && c.DateOfCreation.Minute.Equals(date.Minute) && c.DateOfCreation.Second.Equals(date.Second) && c.Content.Equals(comment.Content)).FirstOrDefault();
            var task = this.taskService.GetById(comment.Task);
            if (task.Comments == null)
            {
                task.Comments = new List<string>();
            }
            task.Comments.Add(comment.Id);
            this.taskService.Update(task.Id, task);
        }
    }
}
