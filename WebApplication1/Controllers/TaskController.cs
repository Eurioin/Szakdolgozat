using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        public TaskController()
        {
            // repo kezelő ide
        }

        public ActionResult Details(string projectId)
        {
            // részletek lekérése
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            // létrehozás
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            // szerkesztés
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string projectId)
        {
            // törlés
            return null;
        }
    }
}
