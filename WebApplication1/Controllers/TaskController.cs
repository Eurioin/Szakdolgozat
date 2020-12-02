using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models.DatabaseModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskService taskService;
        private readonly SubTaskService subTaskService;

        public TaskController(TaskService tService, SubTaskService stService)
        {
            this.taskService = tService;
            this.subTaskService = stService;
        }

        public ActionResult<Task> Details(string taskId)
        {
            var task = this.taskService.GetById(taskId);
            var subs = this.subTaskService.GetAll().Where(t => t.ParentTaksId.Equals(task.Id));
            task.ServerSideTaskList = new List<SubTask>();
            task.ServerSideTaskList.AddRange(subs);
            return task;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(Task t)
        {
            this.taskService.Create(t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Edit(string id, Task t)
        {
            this.taskService.Update(id, t);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Delete(string id)
        {
            var task = this.taskService.GetById(id);
            if (task.NumberOfSubTasks != 0)
            {
                foreach (var item in task.SubTasksIds)
                {
                    this.subTaskService.Remove(item);
                }
            }

            this.taskService.Remove(id);
        }
    }
}
