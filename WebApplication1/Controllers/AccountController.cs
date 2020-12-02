using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Controllers
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
