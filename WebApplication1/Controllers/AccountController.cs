using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models.DatabaseModels;

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
