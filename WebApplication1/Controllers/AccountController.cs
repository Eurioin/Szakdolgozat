using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService accountService;

        public AccountController(AccountService service)
        {
            this.accountService = service;
        }

        // Get users informations by id (GPDR safe probably)
        public ActionResult<Account> Details(string userid)
        {
            return this.accountService.GetById(userid);
        }
    }
}
