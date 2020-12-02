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
    public class AccountController : Controller
    {
        public AccountController()
        {
            // repo kezelő ide
        }

        // Get users informations by id (GPDR safe probably)
        public ActionResult Details(string userid)
        {
            return View();
        }
    }
}
